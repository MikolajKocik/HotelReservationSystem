((): void => {
    const reviews : HTMLElement | null = document.getElementById('reviews-container');
    const dotsContainer : HTMLElement | null = document.getElementById('bottom-nav-dots');
    if (!reviews || !dotsContainer) return;

    const reviewsEl: HTMLElement = reviews;
    const dotsEl: HTMLElement = dotsContainer;
    const items: HTMLElement[] = Array.from(reviewsEl.querySelectorAll<HTMLElement>('.review-item'));

    dotsEl.innerHTML = '';
    items.forEach((_, i: number) => {
        const btn = document.createElement('button') as HTMLButtonElement;
        btn.className = 'dot' + (i === 0 ? ' active' : '');
        btn.dataset.index = String(i);
        btn.setAttribute('aria-label', `Page ${i + 1}`);
        dotsEl.appendChild(btn);
        btn.addEventListener('click', (_ev: MouseEvent) => scrollToIndex(i));
    });

    function scrollToIndex(index: number): void {
        const item: HTMLElement | undefined = items[index];
        if (!item) return;
        item.scrollIntoView({ behavior: 'smooth', block: 'nearest', inline: 'start' });
        const dots: HTMLButtonElement[] = Array.from(dotsEl.querySelectorAll('.dot')) as HTMLButtonElement[];
        dots.forEach((d: HTMLButtonElement) => d.classList.remove('active'));
        if (dots[index]) dots[index].classList.add('active');
    }

    reviewsEl.addEventListener('scroll', () => {
        const rects: DOMRect[] = items.map((it: HTMLElement) => it.getBoundingClientRect());
        const containerRect: DOMRect = reviewsEl.getBoundingClientRect();
        const centerX: number = containerRect.left + containerRect.width / 2;
        let closestIndex: number = 0;
        let closestDist: number = Infinity;
        rects.forEach((r: DOMRect, i: number) => {
            const itemCenter: number = r.left + r.width / 2;
            const dist: number = Math.abs(itemCenter - centerX);
            if (dist < closestDist) {
                closestDist = dist;
                closestIndex = i;
            }
        });
        const dots: HTMLButtonElement[] = Array.from(dotsEl.querySelectorAll('.dot')) as HTMLButtonElement[];
        dots.forEach((d: HTMLButtonElement) => d.classList.remove('active'));
        if (dots[closestIndex]) dots[closestIndex].classList.add('active');
    });
})();