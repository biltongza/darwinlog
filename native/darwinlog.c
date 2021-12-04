#include <os/log.h>

extern void Log(os_log_t log, char *message) {
    os_log(log, "%{public}s", message);
}

extern void LogInfo(os_log_t log, char *message) {
    os_log_info(log, "%{public}s", message);
}

extern void LogDebug(os_log_t log, char *message) {
    os_log_debug(log, "%{public}s", message);
}

extern void LogError(os_log_t log, char *message) {
    os_log_error(log, "%{public}s", message);
}

extern void LogFault(os_log_t log, char *message) {
    os_log_fault(log, "%{public}s", message);
}
