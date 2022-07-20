## Send message from iframe to parent context

1. Create (`ScriptService.cs`)

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

2. Register as scope service (`Program.cs`)

```
builder.Services.AddScoped<ScriptService>();
```

3. Listen window's message (`index.js`)

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

4. Send message

```html
@page "/my-frame"
@using MyWeb.Services
@layout EmptyLayout

<button @onclick="SendMessage">Send Message</button>

@code {

    class Model
    {
        public string Name { set; get; }
        public int Age { set; get; }
    }

    [Inject]
    public ScriptService JS { set; get; }

    private async Task SendMessage(MouseEventArgs args)
    {
        var model = new Model { Name = "wk", Age = 20 };
        await JS.WindowParentPostMessage(model);
    }
}
```