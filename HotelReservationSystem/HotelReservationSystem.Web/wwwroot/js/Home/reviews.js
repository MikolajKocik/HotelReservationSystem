"use strict";
(() => {
    const reviews = document.getElementById('reviews-container');
    const dotsContainer = document.getElementById('bottom-nav-dots');
    const leftBtn = document.getElementById('bottom-nav-left');
    const rightBtn = document.getElementById('bottom-nav-right');
    if (!reviews || !dotsContainer || !leftBtn || !rightBtn)
        return;
    const reviewsEl = reviews;
    const dotsEl = dotsContainer;
    const items = Array.from(reviewsEl.querySelectorAll('.review-item'));
    let currentIndex = 0;
    dotsEl.innerHTML = '';
    items.forEach((_, i) => {
        const btn = document.createElement('button');
        btn.className = 'dot' + (i === 0 ? ' active' : '');
        btn.dataset.index = String(i);
        btn.setAttribute('aria-label', `Page ${i + 1}`);
        dotsEl.appendChild(btn);
        btn.addEventListener('click', (_ev) => scrollToIndex(i));
    });
    function scrollToIndex(index) {
        if (index < 0 || index >= items.length)
            return;
        currentIndex = index;
        const item = items[index];
        if (!item)
            return;
        item.scrollIntoView({ behavior: 'smooth', block: 'nearest', inline: 'start' });
        const dots = Array.from(dotsEl.querySelectorAll('.dot'));
        dots.forEach((d) => d.classList.remove('active'));
        if (dots[index])
            dots[index].classList.add('active');
    }
    leftBtn.addEventListener('click', () => scrollToIndex(currentIndex - 1));
    rightBtn.addEventListener('click', () => scrollToIndex(currentIndex + 1));
    reviewsEl.addEventListener('scroll', () => {
        const rects = items.map((it) => it.getBoundingClientRect());
        const containerRect = reviewsEl.getBoundingClientRect();
        const centerX = containerRect.left + containerRect.width / 2;
        let closestIndex = 0;
        let closestDist = Infinity;
        rects.forEach((r, i) => {
            const itemCenter = r.left + r.width / 2;
            const dist = Math.abs(itemCenter - centerX);
            if (dist < closestDist) {
                closestDist = dist;
                closestIndex = i;
            }
        });
        currentIndex = closestIndex;
        const dots = Array.from(dotsEl.querySelectorAll('.dot'));
        dots.forEach((d) => d.classList.remove('active'));
        if (dots[closestIndex])
            dots[closestIndex].classList.add('active');
    });
})();
//# sourceMappingURL=reviews.js.map