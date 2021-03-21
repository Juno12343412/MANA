using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.EventSystem;

public class Test : ILogContext
{
	ULogger _log;

    public Test(ILog log)
	{
		Debug.Log("로거 생성 : " + log.GetType());
		_log = log.CreateLogger(this);
	}

	public void TestFunc1()
    {
		_log.Message("1");
	}

	public void TestFunc2()
	{
		_log.Message("2");
	}
}