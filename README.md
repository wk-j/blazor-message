## Send message from iframe to parent context

- Create (`ScriptService.cs`)

```csharp
public class ScriptService {
    private readonly IJSRuntime _js;

    public ScriptService(IJSRuntime js) {
        _js = js;
    }

    public async Task WindowParentPostMessage<T>(T model) {
        await _js.InvokeAsync<T>("window.parent.postMessage", model, "*");
    }
}
```

- Register as scope service (`Program.cs`)

```
builder.Services.AddScoped<ScriptService>();
```

- Listen window's message (`index.js`)

```javascript
function bindEvent(element, eventName, eventHandler) {
    if (element.addEventListener) {
        element.addEventListener(eventName, eventHandler, false);
    } else if (element.attachEvent) {
        element.attachEvent('on' + eventName, eventHandler);
    }
}

var iframeEl = document.getElementById('mainframe')

bindEvent(window, 'message', function (e) {
    console.log(e.data)
});
```