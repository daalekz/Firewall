using System;
using System.Reflection;


//https://stackoverflow.com/questions/3167617/determine-if-code-is-running-as-part-of-a-unit-test

/// <summary>
/// Detect if we are running as part of a nUnit unit test.
/// This is DIRTY and should only be used if absolutely necessary 
/// as its usually a sign of bad design.
/// </summary>    
static class UnitTestDetector
{

    private static bool _runningFromNUnit = false;

    static UnitTestDetector()
    {
        foreach (Assembly assem in AppDomain.CurrentDomain.GetAssemblies())
        {
            // Can't do something like this as it will load the nUnit assembly
            // if (assem == typeof(NUnit.Framework.Assert))

            if (assem.FullName.ToLowerInvariant().StartsWith("nunit.framework") || assem.FullName.ToLowerInvariant().StartsWith("unityengine.testtools"))
            {
                _runningFromNUnit = true;
                break;
            }
        }
    }

    public static bool IsRunningFromNUnit
    {
        get { return _runningFromNUnit; }
    }
}