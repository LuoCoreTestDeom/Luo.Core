namespace Luo.Core.Common;

/// <summary>
/// 字符状态
/// </summary>
public class CharState
{
    internal bool jsonStart = false;//以 "{"开始了...
    internal bool setDicValue = false;// 可以设置字典值了。
    internal bool escapeChar = false;//以"\"转义符号开始了
    /// <summary>
    /// 数组开始【仅第一开头才算】，值嵌套的以【childrenStart】来标识。
    /// </summary>
    internal bool arrayStart = false;//以"[" 符号开始了
    internal bool childrenStart = false;//子级嵌套开始了。
    /// <summary>
    /// 【0 初始状态，或 遇到“,”逗号】；【1 遇到“：”冒号】
    /// </summary>
    internal int state = 0;

    /// <summary>
    /// 【-1 取值结束】【0 未开始】【1 无引号开始】【2 单引号开始】【3 双引号开始】
    /// </summary>
    internal int keyStart = 0;
    /// <summary>
    /// 【-1 取值结束】【0 未开始】【1 无引号开始】【2 单引号开始】【3 双引号开始】
    /// </summary>
    internal int valueStart = 0;
    internal bool isError = false;//是否语法错误。

    internal void CheckIsError(char c)//只当成一级处理（因为GetLength会递归到每一个子项处理）
    {
        if (keyStart > 1 || valueStart > 1)
        {
            return;
        }
        //示例 ["aa",{"bbbb":123,"fff","ddd"}] 
        switch (c)
        {
            case '{'://[{ "[{A}]":[{"[{B}]":3,"m":"C"}]}]
                isError = jsonStart && state == 0;//重复开始错误 同时不是值处理。
                break;
            case '}':
                isError = !jsonStart || (keyStart != 0 && state == 0);//重复结束错误 或者 提前结束{"aa"}。正常的有{}
                break;
            case '[':
                isError = arrayStart && state == 0;//重复开始错误
                break;
            case ']':
                isError = !arrayStart || jsonStart;//重复开始错误 或者 Json 未结束
                break;
            case '"':
            case '\'':
                isError = !(jsonStart || arrayStart); //json 或数组开始。
                if (!isError)
                {
                    //重复开始 [""",{"" "}]
                    isError = (state == 0 && keyStart == -1) || (state == 1 && valueStart == -1);
                }
                if (!isError && arrayStart && !jsonStart && c == '\'')//['aa',{}]
                {
                    isError = true;
                }
                break;
            case ':':
                isError = !jsonStart || state == 1;//重复出现。
                break;
            case ',':
                isError = !(jsonStart || arrayStart); //json 或数组开始。
                if (!isError)
                {
                    if (jsonStart)
                    {
                        isError = state == 0 || (state == 1 && valueStart > 1);//重复出现。
                    }
                    else if (arrayStart)//["aa,] [,]  [{},{}]
                    {
                        isError = keyStart == 0 && !setDicValue;
                    }
                }
                break;
            case ' ':
            case '\r':
            case '\n'://[ "a",\r\n{} ]
            case '\0':
            case '\t':
                break;
            default: //值开头。。
                isError = (!jsonStart && !arrayStart) || (state == 0 && keyStart == -1) || (valueStart == -1 && state == 1);//
                break;
        }
        //if (isError)
        //{

        //}
    }
}
