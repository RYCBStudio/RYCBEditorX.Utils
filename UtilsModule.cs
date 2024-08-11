using Prism.Ioc;
using Prism.Modularity;
namespace RYCBEditorX.Utils;
public class UtilsModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
        GlobalConfig.CurrentLogger.Log("Utils模块初始化完成.", module: EnumLogModule.CUSTOM, customModuleName: "Utils模块:初始化");
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterInstance(nameof(LogUtil));
    }
}