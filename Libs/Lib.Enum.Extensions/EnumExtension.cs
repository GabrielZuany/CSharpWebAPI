using System.ComponentModel;

namespace Libs.Lib.Enum;

public static class EnumExtension
{
    public static string GetDescription(this System.Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());

        if (fieldInfo != null)
        {
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
        }

        return value.ToString();
    }
}