/*using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ParticleAnimTrans : AbstractTransAnim
{
    public override void DoEnterAnim()
    {
        Addressables.LoadAssetAsync<GameObject>("TransAnim Particle").Completed += handle =>
        {
            var instance = GameObject.Instantiate(handle.Result);
            instance.SetParent(_transition.transform);
            Framework.Timer.Instance.SetTimer(() => _transition.OnAnimEnterEnd(), 2f);
        };
    }

    public override void DoExitAnim()
    {
    }

    public override void DoHalfTimeAnim()
    {
    }
}*/
