using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Cn  {

    public enum Object { Console, Game }

    /// <summary>
    /// Invokes a Class Method with parameters. This where the magic happens :) 
    /// </summary>
    /// <param name="methodName">Method to Call</param>
    /// <param name="className">Method's Class Name</param>
    /// <param name="stringParams">Method's parameters</param>
    /// <returns>Null</returns>
    public string InvokeStringMethod(string methodName, string className = null, string[] stringParams = null) {
        Type calledType = Type.GetType(className);
        String s = (String)calledType.InvokeMember(methodName, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null,
                    new System.Object[] { stringParams });
        return s;
    }

}
