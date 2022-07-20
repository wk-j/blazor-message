using Microsoft.JSInterop;

namespace MyWeb.Services;

public class ScriptService {
    private readonly IJSRuntime _js;

    public ScriptService(IJSRuntime js) {
        _js = js;
    }

    public async Task WindowParentPostMessage<T>(T model) {
        await _js.InvokeAsync<T>("window.parent.postMessage", model, "*");
    }
}