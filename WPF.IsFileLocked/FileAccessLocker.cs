using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace WPF.IsFileLocked
{
    public class FileAccessLocker
    {
        #region Design pattern "Lazy Singleton"

        private static FileAccessLocker _instance;
        public static FileAccessLocker Instance => LazyInitializer.EnsureInitialized(ref _instance);

        #endregion

        #region Constructor and destructor

        public FileAccessLocker() { SetupDefault(); }

        public void SetupDefault()
        {
            AccessProcessLock = IsDllExists("rstrtmgr");
        }

        #endregion

        /// <summary>
        /// Check access to process lock by rstrtmgr.dll
        /// </summary>
        public bool AccessProcessLock { get; private set; } = false;

        #region DllImport("rstrtmgr.dll)

        // maximum character count of application friendly name.
        private const int CCH_RM_MAX_APP_NAME = 255;
        // maximum character count of service short name.
        private const int CCH_RM_MAX_SVC_NAME = 63;
        // A system restart is not required.
        private const int RmRebootReasonNone = 0;

        /// <summary>
        /// Uniquely identifies a process by its PID and the time the process began. 
        /// An array of RM_UNIQUE_PROCESS structures can be passed
        /// to the RmRegisterResources function.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        struct RM_UNIQUE_PROCESS
        {
            // The product identifier (PID).
            public int dwProcessId;
            // The creation time of the process.
            public System.Runtime.InteropServices.ComTypes.FILETIME ProcessStartTime;
        }

        /// <summary>
        /// Specifies the type of application that is described by
        /// the RM_PROCESS_INFO structure.
        /// </summary>
        enum RM_APP_TYPE
        {
            // The application cannot be classified as any other type.
            RmUnknownApp = 0,
            // A Windows application run as a stand-alone process that
            // displays a top-level window.
            RmMainWindow = 1,
            // A Windows application that does not run as a stand-alone
            // process and does not display a top-level window.
            RmOtherWindow = 2,
            // The application is a Windows service.
            RmService = 3,
            // The application is Windows Explorer.
            RmExplorer = 4,
            // The application is a stand-alone console application.
            RmConsole = 5,
            // A system restart is required to complete the installation because
            // a process cannot be shut down.
            RmCritical = 1000
        }

        /// <summary>
        /// Describes an application that is to be registered with the Restart Manager.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct RM_PROCESS_INFO
        {
            // Contains an RM_UNIQUE_PROCESS structure that uniquely identifies the
            // application by its PID and the time the process began.
            public RM_UNIQUE_PROCESS Process;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_APP_NAME + 1)]
            // If the process is a service, this parameter returns the 
            // long name for the service.
            public string strAppName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_SVC_NAME + 1)]
            // If the process is a service, this is the short name for the service.
            public string strServiceShortName;
            // Contains an RM_APP_TYPE enumeration value.
            public RM_APP_TYPE ApplicationType;
            // Contains a bit mask that describes the current status of the application.
            public uint AppStatus;
            // Contains the Terminal Services session ID of the process.
            public uint TSSessionId;
            // TRUE if the application can be restarted by the 
            // Restart Manager; otherwise, FALSE.
            [MarshalAs(UnmanagedType.Bool)]
            public bool bRestartable;
        }

        /// <summary>
        /// Registers resources to a Restart Manager session. The Restart Manager uses 
        /// the list of resources registered with the session to determine which 
        /// applications and services must be shut down and restarted. Resources can be 
        /// identified by filenames, service short names, or RM_UNIQUE_PROCESS structures
        /// that describe running applications.
        /// </summary>
        /// <param name="pSessionHandle">
        /// A handle to an existing Restart Manager session.
        /// </param>
        /// <param name="nFiles">The number of files being registered</param>
        /// <param name="rgsFilenames">
        /// An array of null-terminated strings of full filename paths.
        /// </param>
        /// <param name="nApplications">The number of processes being registered</param>
        /// <param name="rgApplications">An array of RM_UNIQUE_PROCESS structures</param>
        /// <param name="nServices">The number of services to be registered</param>
        /// <param name="rgsServiceNames">
        /// An array of null-terminated strings of service short names.
        /// </param>
        /// <returns>The function can return one of the system error codes that 
        /// are defined in Winerror.h
        /// </returns>
        [DllImport("rstrtmgr.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int RmRegisterResources(uint pSessionHandle, uint nFiles, string[] rgsFilenames,
            uint nApplications, [In] RM_UNIQUE_PROCESS[] rgApplications, uint nServices, string[] rgsServiceNames);

        /// <summary>
        /// Starts a new Restart Manager session. A maximum of 64 Restart Manager 
        /// sessions per user session can be open on the system at the same time. 
        /// When this function starts a session, it returns a session handle and 
        /// session key that can be used in subsequent calls to the Restart Manager API.
        /// </summary>
        /// <param name="pSessionHandle">
        /// A pointer to the handle of a Restart Manager session.
        /// </param>
        /// <param name="dwSessionFlags">Reserved. This parameter should be 0.</param>
        /// <param name="strSessionKey">
        /// A null-terminated string that contains the session key to the new session.
        /// </param>
        /// <returns></returns>
        [DllImport("rstrtmgr.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int RmStartSession(out uint pSessionHandle, int dwSessionFlags, string strSessionKey);

        /// <summary>
        /// Ends the Restart Manager session. This function should be called by the 
        /// primary installer that has previously started the session by calling the 
        /// RmStartSession function. The RmEndSession function can be called by a 
        /// secondary installer that is joined to the session once no more resources 
        /// need to be registered by the secondary installer.
        /// </summary>
        /// <param name="pSessionHandle">
        /// A handle to an existing Restart Manager session.
        /// </param>
        /// <returns>
        /// The function can return one of the system error codes
        /// that are defined in Winerror.h.
        /// </returns>
        [DllImport("rstrtmgr.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int RmEndSession(uint pSessionHandle);

        /// <summary>
        /// Gets a list of all applications and services that are currently using 
        /// resources that have been registered with the Restart Manager session.
        /// </summary>
        /// <param name="dwSessionHandle">
        /// A handle to an existing Restart Manager session.
        /// </param>
        /// <param name="pnProcInfoNeeded">A pointer to an array size necessary to 
        /// receive RM_PROCESS_INFO structures required to return information for 
        /// all affected applications and services.
        /// </param>
        /// <param name="pnProcInfo">
        /// A pointer to the total number of RM_PROCESS_INFO structures in an array
        /// and number of structures filled.
        /// </param>
        /// <param name="rgAffectedApps">
        /// An array of RM_PROCESS_INFO structures that list the applications and 
        /// services using resources that have been registered with the session.
        /// </param>
        /// <param name="lpdwRebootReasons">
        /// Pointer to location that receives a value of the RM_REBOOT_REASON
        /// enumeration that describes the reason a system restart is needed.
        /// </param>
        /// <returns></returns>
        [DllImport("rstrtmgr.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int RmGetList(uint dwSessionHandle, out uint pnProcInfoNeeded, ref uint pnProcInfo,
            [In, Out] RM_PROCESS_INFO[] rgAffectedApps, ref uint lpdwRebootReasons);
        #endregion

        private bool IsFileExists(string dir, string fileNameShort)
        {
            if (System.IO.Directory.Exists(dir))
            {
                if (!dir.EndsWith(@"\"))
                    dir += @"\";
                if (System.IO.File.Exists(dir + fileNameShort))
                    return true;
            }

            return false;
        }

        private bool IsDllExists(string fileNameShort)
        {
            if (!fileNameShort.EndsWith(".dll") && !fileNameShort.Contains(".dll"))
                fileNameShort += ".dll";

            if (IsFileExists(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), fileNameShort))
                return true;

            if (IsFileExists(Environment.GetFolderPath(Environment.SpecialFolder.System), fileNameShort))
                return true;

            return false;
        }

        public bool IsProcessLock(string fileNameFull, out string message)
        {
            message = string.Empty;
            if (!AccessProcessLock || string.IsNullOrEmpty(fileNameFull))
                return false;

            string sessionkey = Guid.NewGuid().ToString();
            List<Process> processes = new List<Process>();
            int res = RmStartSession(out uint handle, 0, sessionkey);
            if (res != 0)
            {
                message = @"Could not begin restart session!";
                return false;
            }

            try
            {
                uint pnProcInfo = 100, lpdwRebootReasons = RmRebootReasonNone;
                string[] resources = new string[] { fileNameFull };
                RM_PROCESS_INFO[] processInfo = new RM_PROCESS_INFO[pnProcInfo];

                res = RmRegisterResources(handle, (uint)resources.Length, resources, 0, null, 0, null);
                if (res != 0)
                {
                    message = @"Could not register resource!";
                    return false;
                }

                res = RmGetList(handle, out uint pnProcInfoNeeded, ref pnProcInfo, processInfo, ref lpdwRebootReasons);
                if (res == 0)
                {
                    if (pnProcInfo != 0)
                    {
                        var isLock = false;
                        for (int i = 0; i < pnProcInfo; i++)
                        {
                            message = $"Applicaion locking the file: {processInfo[i].strAppName}.";
                            isLock = true;
                        }
                        if (isLock)
                            return true;
                    }
                    else
                    {
                        message = "The specified file is not locked by any process";
                    }
                }
                else
                {
                    message = "Could not list processes locking resource.";
                    return false;
                }

                if (res != 0)
                {
                    message = Marshal.GetLastWin32Error().ToString();
                    return false;
                }

            }
            catch (Exception exception)
            {
                message = exception.Message;
                return false;
            }
            finally
            {
                RmEndSession(handle);
            }

            return false;
        }

        public bool IsProcessLockWithException(string fileNameFull, out string message)
        {
            message = string.Empty;
            if (!AccessProcessLock || string.IsNullOrEmpty(fileNameFull))
                return false;

            string sessionkey = Guid.NewGuid().ToString();
            List<Process> processes = new List<Process>();
            int res = RmStartSession(out uint handle, 0, sessionkey);
            if (res != 0)
            {
                throw new Exception("Could not begin restart session!");
            }

            try
            {
                uint pnProcInfo = 100, lpdwRebootReasons = RmRebootReasonNone;
                string[] resources = new string[] { fileNameFull };
                RM_PROCESS_INFO[] processInfo = new RM_PROCESS_INFO[pnProcInfo];

                res = RmRegisterResources(handle, (uint)resources.Length, resources, 0, null, 0, null);
                if (res != 0)
                {
                    throw new Exception("Could not register resource.");
                }

                res = RmGetList(handle, out uint pnProcInfoNeeded, ref pnProcInfo, processInfo, ref lpdwRebootReasons);
                if (res == 0)
                {
                    if (pnProcInfo != 0)
                    {
                        var isLock = false;
                        for (int i = 0; i < pnProcInfo; i++)
                        {
                            message = $"Applicaion locking the file: {processInfo[i].strAppName}.";
                            isLock = true;
                        }
                        if (isLock)
                            return true;
                    }
                    else
                    {
                        message = "The specified file is not locked by any process";
                    }
                }
                else
                {
                    throw new Exception("Could not list processes locking resource.");
                }

                if (res != 0)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

            }
            catch (Exception exception)
            {
                message = exception.Message;
                throw exception;
            }
            finally
            {
                RmEndSession(handle);
            }

            return false;
        }
    }
}
