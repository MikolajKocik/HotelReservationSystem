document.querySelectorAll(".date").forEach(input => {
    input.addEventListener('click', function (this: HTMLInputElement) {
        if (typeof this.showPicker === 'function') {
            this.showPicker();
        } else {
            console.warn("This browser doesnt support showPicker().");
        }
    });
});
