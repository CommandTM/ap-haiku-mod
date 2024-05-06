using System.Collections;
using BepInEx.Logging;
using DG.Tweening;
using UnityEngine;

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
            if (self.itemID == 1 || self.itemID == 6 || self.itemID == 7 || self.itemID == 8)
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
        TweenSettingsExtensions.SetEase<Tweener>(
            ShortcutExtensions.DOMove(PlayerScript.instance.transform, self.target.position, 1.6f, false), (Ease)2);
        yield return new WaitForSeconds(1.6f);
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
}