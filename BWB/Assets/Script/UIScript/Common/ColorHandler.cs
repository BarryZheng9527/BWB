using UnityEngine;
using System.Collections;

public static class ColorHandler
{
    /*
     * 装备颜色名
     */
    static public string GetEquipColorText(string text, int quality)
    {
        switch (quality)
        {
            case Constant.WHITE:
                return ColorConstant.EQUIP_WHITE + text + ColorConstant.ColorEnd;
            case Constant.GREEN:
                return ColorConstant.EQUIP_GREEN + text + ColorConstant.ColorEnd;
            case Constant.BLUE:
                return ColorConstant.EQUIP_BLUE + text + ColorConstant.ColorEnd;
            case Constant.PURPLE:
                return ColorConstant.EQUIP_PURPLE + text + ColorConstant.ColorEnd;
            case Constant.ORANGE:
                return ColorConstant.EQUIP_ORANGE + text + ColorConstant.ColorEnd;
            default:
                return ColorConstant.EQUIP_WHITE + text + ColorConstant.ColorEnd;
        }
    }
}