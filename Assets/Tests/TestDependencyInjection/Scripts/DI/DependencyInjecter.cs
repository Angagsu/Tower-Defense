using UnityEngine;
using System.Reflection;


public class DependencyInjecter : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] monoBehaviours;



    private void Start()
    {
        StartInjecting();
    }

    public void StartInjecting()
    {
        foreach (var monobehaviour in monoBehaviours)
        {
            Inject(monobehaviour);
        }
    }

    private void Inject(MonoBehaviour monoBehaviour)
    {
        var type = monoBehaviour.GetType();

        var methodsInfo = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.FlattenHierarchy | BindingFlags.Instance);

        foreach (var methodInfo in methodsInfo)
        {
            if (!methodInfo.IsDefined(typeof(InjectAttribute)))
            {
                continue;
            }

            var parametersInfo = methodInfo.GetParameters();
            var args = new object[parametersInfo.Length];


            for (int i = 0; i < parametersInfo.Length; i++)
            {
                var argType = parametersInfo[i].ParameterType;
                var arg = ServiceLocator.GetServiceByType(argType);
                args[i] = arg;
            }

            methodInfo.Invoke(monoBehaviour, args);
        }
    } 
}
