using System.Collections;
using BepInEx.Logging;
using DG.Tweening;
using FMOD;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HaikuAP;

public class LocationMachine
{
    public static ManualLogSource Logging = new ManualLogSource("Location Machine");
    private static long _baseID = 0;

    public static void Init()
    {
        On.UnlockTutorial.PowerUpSequence += _apVerUnlockTutorial;
        On.PickupItem.TriggerPickup += _apVerTriggerPickup;
        On.PickupWrench.TriggerPickup += _apVerTriggerWrench;
        On.PickupBulb.TriggerPickup += _apVerTriggerBulb;
        On.e7UpgradeShop.UpgradeSequence += _apVerUpgradeSequence;
        On.PowerCell.OnTriggerEnter2D += _apVerTriggerEnter;
        On.ShopTrigger.ConfirmPurchase += _apVerConfirmPurchase;
        
        On.PowerCell.Start += _apVerPCStart;
    }

    public static void UpdateID(long val)
    {
        _baseID = val;
    }
    
    private static void _sendLocation(long id)
    {
        Logging.LogInfo(APPlugin.apSession.Locations.GetLocationNameFromId(_baseID + id));
        APPlugin.apSession.Locations.CompleteLocationChecks(_baseID + id);
    }
    
    private static void _apVerPCStart(On.PowerCell.orig_Start orig, PowerCell self)
    {
        self.coll = self.GetComponent<CircleCollider2D>();
        self.anim = self.GetComponentInChildren<Animator>();
        self.sr = self.GetComponentInChildren<SpriteRenderer>();
        if (SaveHijacker.CollectedPowerCell(self.saveID))
        {
            self.coll.enabled = false;
            self.anim.enabled = false;
            self.sr.enabled = false;
            return;
        }
        self.PlayLoop();
    }

    private static IEnumerator _apVerUnlockTutorial(On.UnlockTutorial.orig_PowerUpSequence orig, UnlockTutorial self)
    {
        yield return new WaitForSeconds(0.15f);
        self.PlayPowerUpSFX();
        CameraBehavior.instance.Shake(4f, 0.1f);
        yield return new WaitForSeconds(4f);
        self.PlayPowerUpFinish();
        CameraBehavior.instance.Shake(0.2f, 0.2f);
        if (self.bomb)
        {
            _sendLocation(8);
        }
        if (self.roll)
        {
            _sendLocation(1);
        }
        if (self.grapple)
        {
            _sendLocation(6);
        }
        if (self.doublejump)
        {
            _sendLocation(5);
        }
        if (self.walljump)
        {
            _sendLocation(0);
        }
        if (self.teleport)
        {
            _sendLocation(7);
        }
        yield return new WaitForSeconds(1f);
        if (!GameManager.instance.corruptMode)
        {
            PlayerHealth.instance.ReplenishHealth();
        }
        ManaManager.instance.RemoveHeat(271f);
        self.UnfreezePlayer();
        yield break;
    }
    
    private static void _apVerTriggerPickup(On.PickupItem.orig_TriggerPickup orig, PickupItem self)
    {
        if (!self.triggerPin)
        {
            GameManager.instance.worldObjects[self.saveID].collected = true;
        }
        Object.Instantiate<GameObject>(self.collectParticles, self.transform.position, Quaternion.identity);

        if (!self.triggerChip && !self.triggerChipSlot && !self.triggerCoolant)
        {
            if (self.itemID == 1 || self.itemID == 6)
            {
                _sendLocation(IDTranslate.ItemIDToAPID[self.itemID]+37);
            }
        }
        if (self.triggerChip)
        {
            _sendLocation(IDTranslate.ChipIdentToAPID[self.chipIdentifier]);
        }
        
        SoundManager.instance.PlayOneShot(self.pickupSFXPath);
        self.gameObject.SetActive(false);
    }
    
    private static void _apVerTriggerWrench(On.PickupWrench.orig_TriggerPickup orig, PickupWrench self)
    {
        GameManager.instance.worldObjects[self.saveID].collected = true;
        _sendLocation(9);
        Object.Instantiate<GameObject>(self.collectParticles, self.transform.position, Quaternion.identity);
        SoundManager.instance.PlayOneShot(self.pickupSFXPath);
        self.sr.enabled = false;
        self.interact.SetActive(false);
        self.particles.SetActive(false);
    }
    
    private static void _apVerTriggerBulb(On.PickupBulb.orig_TriggerPickup orig, PickupBulb self)
    {
        _sendLocation(2);
        Object.Instantiate<GameObject>(self.collectParticles, self.transform.position, Quaternion.identity);
        SoundManager.instance.PlayOneShot(self.pickupSFXPath);
        self.interact.SetActive(false);
        self.bulbObject.SetActive(false);
    }
    
    private static IEnumerator _apVerUpgradeSequence(On.e7UpgradeShop.orig_UpgradeSequence orig, e7UpgradeShop self, bool firewater)
    {
        if (firewater)
        {
            self.grillAnim.SetBool("laughing", true);
        }
        else
        {
            self.coolerAnim.SetBool("laughing", true);
        }
        yield return null;
        PlayerScript.instance.canFlip = false;
        PlayerScript.instance.disableControls = true;
        PlayerScript.instance.RollOn();
        self.anim.SetTrigger("extend");
        if (firewater)
        {
            self.anim.SetTrigger("fire");
            _sendLocation(3);
        }
        else
        {
            self.anim.SetTrigger("water");
            _sendLocation(4);
        }
        yield return new WaitForSeconds(1.5f);
        self.PlayParticles(firewater);
        self.PlaySound(firewater);
        yield return new WaitForSeconds(4f);
        self.StopParticles(firewater);
        self.anim.SetTrigger("retract");
        PlayerScript.instance.canFlip = true;
        PlayerScript.instance.disableControls = false;
        PlayerScript.instance.RollOff();
        if (firewater)
        {
            self.grillAnim.SetBool("laughing", false);
        }
        else
        {
            self.coolerAnim.SetBool("laughing", false);
        }
        self.PlaySuccessSound();
        self.successParticles.Play();
        yield break;
    }
    
    private static void _apVerTriggerEnter(On.PowerCell.orig_OnTriggerEnter2D orig, PowerCell self, object collision)
    {
        if (((Collider2D)collision).CompareTag("Player"))
        {
            self.coll.enabled = false;
            self.StartCoroutine(self.RemoveHeat());
            self.anim.SetTrigger("collect");
            SaveHijacker.SentPowerCells.Add(self.saveID);
            _sendLocation(IDTranslate.PowerCellIDToAPID[self.saveID]);
            self.StopLoop();
            SoundManager.instance.PlayOneShot(self.collectSoundEffect);
            self.collectParticles.Play();
        }
    }
    
    private static void _apVerConfirmPurchase(On.ShopTrigger.orig_ConfirmPurchase orig, ShopTrigger self)
    {
        GameManager.instance.worldObjects[self.saveIDHolder].collected = true;
        self.buttonHolder.SetActive(false);
        InventoryManager.instance.SpendSpareParts(self.priceHolder);
        if (self.itemTypeSelected == "item")
        {
            switch (self.itemIDHolder)
            {
                case 0:
                    _sendLocation(76);
                    break;
                case 3:
                    if (!SaveHijacker.FirstShopHFSent)
                    {
                        _sendLocation(72);
                        SaveHijacker.FirstShopHFSent = true;
                    }
                    else
                    {
                        _sendLocation(73);
                    }
                    break;
                case 7:
                    _sendLocation(81);
                    break;
                case 8:
                    _sendLocation(82);
                    break;
            }
        }
        if (self.itemTypeSelected == "chip")
        {
            _sendLocation(IDTranslate.ChipIdentToAPID[GameManager.instance.chip[self.itemIDHolder].identifier]);
        }
        if (self.itemTypeSelected == "chipSlot")
        {
            if (self.itemIDHolder == 6)
            {
                _sendLocation(90);
            }
            else
            {
                _sendLocation(89);
            }
        }
        if (self.itemTypeSelected == "marker")
        {
            CameraBehavior.instance.ShowLeftCornerUI(self.itemImage.sprite, "_MARKER", "", 2f);
        }
        SoundManager.instance.PlayOneShot("event:/UI/UI Success");
        self.Invoke("PlayPurchaseSound", 0.25f);
        self.areYouSureCanvas.SetActive(false);
        self.shopCanvas.SetActive(true);
        if (self.allItemsSold())
        {
            self.CloseShop(false);
            self.gameObject.SetActive(false);
            self.ChangeDialogueTriggersToAllItemsSold();
            return;
        }
        self.AssignFirstItemToEvents();
    }
}