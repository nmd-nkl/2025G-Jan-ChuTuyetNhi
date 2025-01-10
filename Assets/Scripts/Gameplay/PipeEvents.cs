using System;

public static class PipeEvents {
    public static event Action<int> OnPipeStatusChanged;
    public static void RegisterStatusChanged(Action<int> callback) {
        OnPipeStatusChanged += callback;
    }
    public static void UnregisterStatusChanged(Action<int> callback) {
        OnPipeStatusChanged -= callback;
    }
    public static void TriggerStatusChanged(int status) {
        OnPipeStatusChanged?.Invoke(status);
    }
}
