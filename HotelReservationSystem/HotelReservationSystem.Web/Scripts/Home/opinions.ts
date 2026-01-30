(() : void => {
    interface Opinion { 
        rating: number; 
        comment: string; 
        guestFirstName?: string; 
        guestLastName?: string 
    }

    async function loadOpinions(): Promise<void> {
        try {
            const res = await fetch('/Opinion');
            if (!res.ok) return;
            const opinions: Opinion[] = await res.json();
            const items = Array.from(document.querySelectorAll<HTMLElement>('.review-item'));

            opinions.slice(0, items.length).forEach((op, i) => {
                const item = items[i];
                if (!item) return;
                const starsEl = item.querySelector<HTMLElement>('.rating-review .rating-stars') ?? item.querySelector<HTMLElement>('.rating-review span');
                if (starsEl) starsEl.textContent = renderStars(Math.round(op.rating));
                const commentEl = item.querySelector<HTMLElement>('.review-text p');
                if (commentEl) commentEl.textContent = op.comment ?? '';
                const nameEl = item.querySelector<HTMLElement>('.rating-review p');
                if (nameEl) nameEl.textContent = op.guestFirstName ? `${op.guestFirstName} ${op.guestLastName ?? ''}` : '';
            });

            const avg = opinions.length ? Math.round(opinions.reduce((s, o) => s + o.rating, 0) / opinions.length) : 0;
            const hotelStars = document.getElementById('hotel-stars');
            if (hotelStars) hotelStars.textContent = renderStars(avg);
        } catch (e) {
            console.error(e);
        }
    }

    function renderStars(n: number): string {
        const clamped = Math.max(0, Math.min(5, n));
        const full = '★'.repeat(clamped);
        const empty = '☆'.repeat(5 - clamped);
        return [full, empty].filter(Boolean).join(' ');
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', loadOpinions);
    else loadOpinions();
})();