using System;
using System.Collections.Generic;

namespace HaikuAP;

public class IDTranslate
{
    public static Dictionary<String, long> ChipIdentToAPID = new Dictionary<string, long>()
    {
        {"r_CritChance", 10},
        {"r_ElectricTrail", 11},
        {"r_ShockWave", 12},
        {"r_ShockProjectile", 13},
        {"r_ElectricOrbs1", 14},
        {"b_AutoBomb", 15},
        {"b_FastHeal", 16},
        {"g_RollSpeed", 17},
        {"b_RollSaw", 18},
        {"b_FastCooldown", 19},
        {"g_AutoRoll", 20},
        {"b_LongerInvincibility", 21},
        {"g_AutoHeal", 22},
        {"g_MoreMoney", 23},
        {"r_BulbProjectile", 24},
        {"b_BlinkDistance", 25},
        {"r_NoKnockback", 26},
        {"r_SharpDash", 27},
        {"b_ReduceMoneyDropped", 28},
        {"g_ExtraFireRes", 29},
        {"r_QuickAttack", 30},
        {"r_AttackRange", 31},
        {"b_MoneyMagnet", 32},
        {"b_IncreaseHealth1", 33},
        {"g_Magnetism", 34},
        {"g_MapHelper", 35},
        {"r_IncreasedBombDamage", 36},
        {"r_LessBombHeatCost", 37}
    };

    public static Dictionary<long, String> APIDToChipIdent = new Dictionary<long, string>();

    public static Dictionary<int, long> ItemIDToAPID = new Dictionary<int, long>()
    {
        { 2, 39 },
        { 3, 40 },
        { 0, 41 },
        { 1, 42 },
        { 6, 43 },
        { 7, 44 },
        { 8, 45 }
    };
    
    public static Dictionary<long, int> APIDToItemID = new Dictionary<long, int>();

    public static Dictionary<int, long> PowerCellIDToAPID = new Dictionary<int, long>()
    {
        {0, 50},
        {1, 59},
        {2, 60},
        {3, 44},
        {4, 43},
        {5, 46},
        {6, 45},
        {7, 56},
        {8, 53},
        {9, 47},
        {10, 61},
        {11, 40},
        {12, 54},
        {13, 57},
        {14, 38},
        {15, 48},
        {16, 42},
        {17, 41},
        {18, 58},
        {19, 39},
        {20, 52},
        {21, 49},
        {22, 51},
        {23, 55}
    };
    
    public static Dictionary<int, long> PickupIDToAPID = new Dictionary<int, long>()
    {
        {46, 62},
        {56, 63},
        {10, 64},
        {11, 65},
        {6, 66},
        {47, 67},
        {48, 68},
        {30, 69},
        {32, 70},
        {0, 74},
        {49, 76},
        {52, 77},
        {63, 83},
        {62, 84},
        {61, 85},
        {43, 86},
        {44, 87},
        {45, 88}
    };

    public static void InitReverseDicts()
    {
        foreach (string key in ChipIdentToAPID.Keys)
        {
            APIDToChipIdent.Add(ChipIdentToAPID[key], key);
        }

        foreach (int key in ItemIDToAPID.Keys)
        {
            APIDToItemID.Add(ItemIDToAPID[key], key);
        }
    }
}