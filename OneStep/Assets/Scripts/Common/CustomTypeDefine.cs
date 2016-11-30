using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;


public static class GlobalTypeDefine
{
    //public static VersionInfo VERSION_INFO = new VersionInfo("com.IGEE.CodeReset", "CodeReset", 1, 0, 1);
    public static string COMMIT_CODE_AND_DATE = "86d9d6c Wed Apr 20 17:02:47 2016 +0800";

    // scene names
    public const string sm_mainSceneName = "Main";
    public const string sm_battleSceneName = "Battle";
    public const string sm_characterSceneName = "Character";

    // layer names
    public const string sm_defaultLayerName = "Default";                    // index = 0
    public const string sm_transparentFXLayerName = "TransparentFX";        // index = 1
    public const string sm_IgnoreRayCastLayerName = "Ignore Raycast";       // index = 2
    public const string sm_waterLayerName = "Water";                        // index = 4
    public const string sm_uiLayerName = "UI";                              // index = 5   
    public const string sm_foregroundLayerName = "ForeLayer";               // index = 8
    public const string sm_backgroundLayerName = "BackLayer";               // index = 9
    public const string sm_middleLayerName = "MiddleLayer";                 // index = 10                 
    public const string sm_effectLayerName = "EffectLayer";                 // index = 11
    public const string sm_TransportUnitLayerName = "UILayer";              // index = 12

    // extension
    public const string META_EXTENSION = ".meta";
    public const string TXT_EXTENSION = ".txt";
    public const string JSON_EXTENSION = ".json";
    public const string PREFAB_EXTENSION = ".prefab";
    public const string BYTES_EXTENSION = ".bytes";
    public const string AUDIO_EXTENSION = ".mp3";
    public const string SCENE_EXTENSION = ".unity";
    public const string PNG_EXTENSION = ".png";
    public const string TGA_EXTENSION = ".tga";
    public const string TERRAINDATA_EXTENSION = ".asset";
    public const string ANIMATOR_EXTENSION = ".controller";

    // sprite path
    public const string FOOD_SPRITE_PATH = "UI/Texture/Icon/01_SmallIcon/icon_01_002";
    public const string WOOD_SPRITE_PATH = "UI/Texture/Icon/01_SmallIcon/icon_01_004";
    public const string GOLD_SPRITE_PATH = "UI/Texture/Icon/01_SmallIcon/icon_01_006";
    public const string IRON_SPRITE_PATH = "UI/Texture/Icon/01_SmallIcon/icon_01_003";
    public const string ROCK_SPRITE_PATH = "UI/Texture/Icon/01_SmallIcon/icon_01_005";

    public const string SHADOW_PREFAB_NAME = "Shadow";
    public const string EQUIP_PREFAB_NAME = "Equipment";
    public const string SOLDIERUNIT_PREFAB_NAME = "SoldierUnit";

    // 時間常數
    public const int ONE_SECOND_MILLISECOND = 1000;
    public const int ONE_MINUTE_SECOND = 60;
    public const int ONE_HOUR_SECOND = ONE_MINUTE_SECOND * 60;
    public const int ONE_DAY_SECOND = ONE_HOUR_SECOND * 24;
    public const int ONE_WEEK_SECOND = ONE_DAY_SECOND * 7;
    public const long TICKS_IN_ONE_SECOND = 10000000;

    // 變數常數
    public const int VALUE_PERCENTAGE = 100;
    public const int ONE_THOUSAND = 1000;
    public const int ONE_MILLION = 1000000;

    // 字串常數
    public const string STRING_COMMA = ",";
    public const string STRING_PERCENTAGE = "%";
    public const string STRING_SLASH = "/";
    public const string STRING_ONE_THOUSAND_ABBREVIATION = "K";
    public const string STRING_ONE_MILLION_ABBREVIATION = "M";
    public const string STRING_NUMBER_FRONT = "X ";

    // 顏色常數
    public const string STRING_COLOR_BACK = "</color>";
    public const string STRING_COLOR_RED_FRONT = "<color=red>";

    // 屬性常數
    public const int MAX_PLAYER_NAME_LEN = 32;                      // 玩家名字長度上限
    public const int MAX_GUILD_NAME_LEN = 32;                       // 公會名字長度上限
    public const int MAX_GUILD_CODE_LEN = 3;                        // 公會標籤長度上限
    public const int MAX_ITEM_CATEGORY = 5000;                      // 物品種類上限
    public const int MAX_ITEM_STACK_COUNT = 999;                    // 物品堆疊上限
    public const int MAX_BUILDING_AMOUNT = 25;                      // 建物數量上限

    // Table表常數
    public const int RESOURCES_QUANTITY_LV_DATA_LENGTH = 6;

    /* Data Members */

    // pool manager names
    public static string[] sm_spawnPoolName =
    {
        "spawn_pool_gameobject",
        "spawn_pool_audio",
        "spawn_pool_gui_battle",
        "spawn_pool_particle",
        "spawn_pool_gui_city",
        "spawn_pool_traffic_route",
        "spawn_pool_soldier_unit",
        "spawn_pool_equipment",
    };
    //eAnimationEventKind 對應func name
    public static string[] sm_animEventKindFuncName =
    {
        "",
        "",
        "effectTrigger",
        "funcTrigger",
    };
    // 測試用的控制flag
    public static bool sm_showPanels = true;
#if SHOW_LOGIN_LIST
    public static bool sm_isPlayStandalone = true;
#endif

}
