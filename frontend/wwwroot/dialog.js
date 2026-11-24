window.warehouseDialog = {
    open(dialog) {
        if (!dialog?.isConnected || dialog.open)
            return;

        dialog.addEventListener("cancel", event => {
            event.preventDefault();
            dialog.querySelector("[data-modal-dismiss]")?.click();
        });
        dialog.showModal();
    },

    close(dialog) {
        if (dialog?.open)
            dialog.close();
    }
};
