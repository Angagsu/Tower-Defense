using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


public class SystemInformation : MonoBehaviour
{
	// Application
	public string UnityVersion => $"Unity Version:   {Application.unityVersion}";
	public string InstallMode 	=> $"Install Mode:   {Application.installMode}";
	public string SandboxType 	=> $"Sandbox Type:   {Application.sandboxType}";

	public string[] ApplicationStats => new string[]
	{
	 UnityVersion,
	 InstallMode,
	 SandboxType
	};

    // System
    public string 	OperatingSystem => $"Operating System:   {SystemInfo.operatingSystem}";
	public string 	Platform 		=> $"Platform:   {Application.platform}";
	public string 	SystemLanguage 	=> $"System Language:    {Application.systemLanguage}";
	public string 	DeviceName 		=> $"Device Name:   {SystemInfo.deviceName}";
	public string 	DeviceType 		=> $"Device Type:   {SystemInfo.deviceType}";
	public string 	DeviceModel 	=> $"Device Model:   {SystemInfo.deviceModel}";
	public string 	CpuType 		=> $"CPU Type:   {SystemInfo.processorType}";
	public string 	CpuCount 		=> $"CPU Count:   {SystemInfo.processorCount}";
    
    public string[] SystemStats => new string[]
	{
	 OperatingSystem,
	 Platform,
	 SystemLanguage,
	 DeviceName,
	 DeviceType,
	 DeviceModel,
	 CpuType,
	 CpuCount
	};

    // Memory
    public string 	SystemMemoryMB 			=> $"System Memory:   {SystemInfo.systemMemorySize} MB";
	public string	TotalAllocatedMemoryMB	=> $"Total Allocated Memory:   {(Profiler.GetTotalAllocatedMemoryLong() / (1024 * 1024))} MB";
	public string	TotalReservedMemoryMB	=> $"Total Reserved Memory:   {(Profiler.GetTotalReservedMemoryLong() / (1024 * 1024))} MB";
	public string	MonoUsedMemoryMB		=> $"Mono Used Memory:   {(Profiler.GetMonoUsedSizeLong() / (1024 * 1024))} MB";
	public string	MonoHeapMemoryMB		=> $"Mono Heap Memory:   {(Profiler.GetMonoHeapSizeLong() / (1024 * 1024))} MB";
    public string[] MemoryStats => new string[]
    {
    	 SystemMemoryMB,
    	 TotalAllocatedMemoryMB,
    	 TotalReservedMemoryMB,
    	 MonoUsedMemoryMB,
    	 MonoHeapMemoryMB
    };

    // Runtime
    public string 	PlayTime 			=> $"Play Time:   {Time.realtimeSinceStartup.ToString("F3", CultureInfo.InvariantCulture)}";
	public string 	LevelPlayTime 		=> $"Level Play Time:   {Time.timeSinceLevelLoad.ToString("F3", CultureInfo.InvariantCulture)}";
	public string 	CurrentScene 		=> $"Current Scene:   {SceneManager.GetActiveScene().name}";
	public string 	QualityLevel 		=> $"Quality Level:   {QualitySettings.names[QualitySettings.GetQualityLevel()]} ({QualitySettings.GetQualityLevel()})";
	public string 	TargetFrameRate 	=> $"Target Frame Rate:   {Application.targetFrameRate}";
	public string 	VSyncCount 			=> $"VSync Count:   {QualitySettings.vSyncCount}";
	public string 	RenderFrameInterval => $"Render Frame Interval:   {OnDemandRendering.renderFrameInterval}";
	public string 	EffectiveFrameRate	=> $"Effective Frame Rate:   {OnDemandRendering.effectiveRenderFrameRate}";
    
    public string[] RuntimeStats => new string[]
	{
	 PlayTime,
	 LevelPlayTime,
	 CurrentScene,
	 QualityLevel,
	 TargetFrameRate,
	 VSyncCount,
	 RenderFrameInterval,
	 EffectiveFrameRate
	};

    // Display
    public string	Resolution 	  => $"Resolution:   {Screen.width}x{Screen.height}";
	public string	RefreshRateHz => $"Refresh Rate:   {Screen.currentResolution.refreshRateRatio.value.ToString("F2", CultureInfo.InvariantCulture)} Hz";
	public string	DPI 		  => $"DPI:   {(Screen.dpi > 0f ? Screen.dpi.ToString("F0") : "Unknown")}";
	public string	Orientation   => $"Orientation:   {Screen.orientation}";
	public string	SafeArea 	  => $"Safe Area:   {Screen.safeArea}";	
	public string[] DisplayStats => new string[]
	{
		 Resolution,
		 RefreshRateHz,
		 DPI,
		 Orientation,
		 SafeArea
	};

    // Graphics
    public string  GraphicsDeviceName 	  => $"Graphics Device Name:   {SystemInfo.graphicsDeviceName}";
	public string  GraphicsVendor 		  => $"Graphics Vendor:   {SystemInfo.graphicsDeviceVendor}";
	public string  GraphicsApi 		  => $"Graphics API:   {SystemInfo.graphicsDeviceType}";
	public string  GraphicsVersion 	  => $"Graphics Version:   {SystemInfo.graphicsDeviceVersion}";
	public string  GraphicsMemoryMB 	  => $"Graphics Memory:   {SystemInfo.graphicsMemorySize} MB";
	public string  MaxTextureSize 		  => $"Max Texture Size:   {SystemInfo.maxTextureSize}";
	public string  MaxArrayTextureSlices => $"Max Array Texture Slices:   {SystemInfo.maxTextureArraySlices}";
	public string  NPOTSupport 		  => $"NPOT Support:   {SystemInfo.npotSupport}";
	public string  RenderTargetCount 	  => $"Render Target Count:   {SystemInfo.supportedRenderTargetCount}";
	public string  ShaderLevel			  => $"Shader Level:   {SystemInfo.graphicsShaderLevel}";
	public string[] GraphicsStats => new string[]
	{
		 GraphicsDeviceName,
		 GraphicsVendor,
		 GraphicsApi,
		 GraphicsVersion,
		 GraphicsMemoryMB,
		 MaxTextureSize,
		 MaxArrayTextureSlices,
		 NPOTSupport,
		 RenderTargetCount,
		 ShaderLevel
	};

	public List<string[]> AllStats => new List<string[]>
	{
		ApplicationStats,
        SystemStats,
		MemoryStats,
		RuntimeStats,
		DisplayStats,
		GraphicsStats
	};

    private void Awake()
    {
        
    }
}
