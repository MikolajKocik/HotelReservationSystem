"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
(() => {
    function loadOpinions() {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                const res = yield fetch('/Opinion');
                if (!res.ok)
                    return;
                const opinions = yield res.json();
                const items = Array.from(document.querySelectorAll('.review-item'));
                opinions.slice(0, items.length).forEach((op, i) => {
                    var _a, _b, _c;
                    const item = items[i];
                    if (!item)
                        return;
                    const starsEl = (_a = item.querySelector('.rating-review .rating-stars')) !== null && _a !== void 0 ? _a : item.querySelector('.rating-review span');
                    if (starsEl)
                        starsEl.textContent = renderStars(Math.round(op.rating));
                    const commentEl = item.querySelector('.review-text p');
                    if (commentEl)
                        commentEl.textContent = (_b = op.comment) !== null && _b !== void 0 ? _b : '';
                    const nameEl = item.querySelector('.rating-review p');
                    if (nameEl)
                        nameEl.textContent = op.guestFirstName ? `${op.guestFirstName} ${(_c = op.guestLastName) !== null && _c !== void 0 ? _c : ''}` : '';
                });
                const avg = opinions.length ? Math.round(opinions.reduce((s, o) => s + o.rating, 0) / opinions.length) : 0;
                const hotelStars = document.getElementById('hotel-stars');
                if (hotelStars)
                    hotelStars.textContent = renderStars(avg);
            }
            catch (e) {
                console.error(e);
            }
        });
    }
    function renderStars(n) {
        const clamped = Math.max(0, Math.min(5, n));
        const full = '★'.repeat(clamped);
        const empty = '☆'.repeat(5 - clamped);
        return [full, empty].filter(Boolean).join(' ');
    }
    if (document.readyState === 'loading')
        document.addEventListener('DOMContentLoaded', loadOpinions);
    else
        loadOpinions();
})();
//# sourceMappingURL=opinions.js.map