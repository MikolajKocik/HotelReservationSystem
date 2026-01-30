(() : void => {
    const scrollBtn : HTMLElement | null = document.getElementById('scroll-up-button');
    if (scrollBtn) {
        scrollBtn.addEventListener('click', function(this: HTMLElement, ev: MouseEvent) : void {
            window.scrollTo(
                { 
                    top: 0 as number | undefined,
                    behavior: 'smooth' as ScrollBehavior | undefined 
                });
        });
    }
})();