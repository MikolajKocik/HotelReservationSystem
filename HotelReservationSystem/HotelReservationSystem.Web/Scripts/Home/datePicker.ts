((): void => {
    const inputs: NodeListOf<HTMLInputElement> = document.querySelectorAll<HTMLInputElement>(".date");

    inputs.forEach((input: HTMLInputElement) => {
        input.addEventListener('click', function (this: HTMLInputElement): void {
            if ('showPicker' in this && typeof (this as any).showPicker === 'function') {
                (this as any).showPicker();
            } else {
                console.warn("This browser doesnt support showPicker().");
            }
        });
    });
})();
