using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MadAngelFilms.SrtEditor.UI.Extensions;

internal static class ControlExtensions
{
    public static Task InvokeAsync(this Control control, Action action)
    {
        if (control.InvokeRequired)
        {
            return control.InvokeAsyncInternal(action);
        }

        action();
        return Task.CompletedTask;
    }

    private static Task InvokeAsyncInternal(this Control control, Action action)
    {
        var completionSource = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        control.BeginInvoke(new MethodInvoker(() =>
        {
            try
            {
                action();
                completionSource.SetResult();
            }
            catch (Exception exception)
            {
                completionSource.SetException(exception);
            }
        }));

        return completionSource.Task;
    }
}
