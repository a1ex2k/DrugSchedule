﻿using Blazorise;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.Components.Common;

public partial class CustomAlert
{
    private Modal _modal = default!;
    private bool _isInfo;
    private bool _showCancel = default!;
    private TaskCompletionSource<ModalResult> _modalTask = default!;

    [Parameter] public RenderFragment AlertBody { get; set; } = default!;

    [Parameter] public string AlertText { get; set; } = default!;

    [Parameter] public string Title { get; set; } = default!;

    public enum ModalResult
    {
        Yes,
        No,
        Cancel
    }

    public async Task<ModalResult> ShowYesNo()
    {
        _isInfo = false;
        _modalTask = new TaskCompletionSource<ModalResult>();
        _showCancel = false;

        return await ShowAlertAsync();
    }

    public async Task<ModalResult> ShowYesNo(string body, string title, bool showCancel = false)
    {
        Title = title;
        AlertText = body;
        _isInfo = false;
        _showCancel = showCancel;
        return await ShowAlertAsync();
    }

    public async Task ShowOk()
    {
        _isInfo = true;
        _showCancel = false;
        await ShowAlertAsync();
    }

    public async Task ShowOk(string body, string title)
    {
        Title = title;
        AlertText = body;
        _isInfo = true;
        _showCancel = false;

        await ShowAlertAsync();
    }

    private async Task<ModalResult> ShowAlertAsync()
    {
        _modalTask = new TaskCompletionSource<ModalResult>();
        await _modal.Show();
        var result = await _modalTask.Task;
        await _modal.Hide();
        return result;
    }

    private void YesClick()
    {
        _modalTask.SetResult(ModalResult.Yes);
    }

    private void NoClick()
    {
        _modalTask.SetResult(ModalResult.No);
    }

    private void CancelClick()
    {
        _modalTask.SetResult(ModalResult.Cancel);
    }

    private Task OnModalClosing(ModalClosingEventArgs e)
    {
        e.Cancel = e.CloseReason != CloseReason.UserClosing && !_isInfo;
        return Task.CompletedTask;
    }
}