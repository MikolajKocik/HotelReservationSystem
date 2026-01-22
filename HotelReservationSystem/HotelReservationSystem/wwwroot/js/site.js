document.querySelectorAll(".date").forEach(input => {
    input.addEventListener('click', function () {
        if (this.showPicker()) {
            this.showPicker();
        }
    });
});
