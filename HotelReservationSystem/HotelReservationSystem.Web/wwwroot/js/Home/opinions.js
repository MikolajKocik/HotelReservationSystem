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
                const carouselInner = document.getElementById('carousel-inner');
                const carouselIndicators = document.getElementById('carousel-indicators');
                if (!carouselInner)
                    return;
                if (opinions.length === 0) {
                    carouselInner.innerHTML = `
                    <div class="carousel-item active">
                        <div class="review-item card h-100 border-0 shadow-sm rounded-3">
                            <div class="card-body bg-light text-center p-4">
                                <p class="review-text m-0 text-dark">Brak opinii</p>
                            </div>
                        </div>
                    </div>`;
                    return;
                }
                carouselInner.innerHTML = '';
                if (carouselIndicators)
                    carouselIndicators.innerHTML = '';
                opinions.forEach((op, i) => {
                    var _a, _b;
                    // Add indicator
                    if (carouselIndicators) {
                        const activeClass = i === 0 ? 'class="active" aria-current="true"' : '';
                        carouselIndicators.insertAdjacentHTML('beforeend', `<button type="button" data-bs-target="#opinionsCarousel" data-bs-slide-to="${i}" ${activeClass} aria-label="Slide ${i + 1}" style="background-color: #23405c;"></button>`);
                    }
                    // Add slide
                    const activeItemClass = i === 0 ? 'active' : '';
                    const stars = renderStars(Math.round(op.rating));
                    const guestName = op.guestFirstName ? `${op.guestFirstName} ${(_a = op.guestLastName) !== null && _a !== void 0 ? _a : ''}` : 'Anonim';
                    const slideHtml = `
                    <div class="carousel-item ${activeItemClass}">
                        <div class="review-item card h-100 border-0 shadow-sm rounded-3">
                            <div class="rating-review-section card-header d-flex align-items-center justify-content-between border-0">
                                <div class="rating-review">
                                    <span class="rating-stars text-warning">${stars}</span>
                                    <p class="m-0 text-white-50 small mt-1">${guestName}</p>
                                </div>
                                <div class="rating-review-icon rounded-circle bg-white d-flex align-items-center justify-content-center shadow-sm">
                                    <svg width="24" height="24" viewBox="0 0 640 640" fill="#23405c">
                                        <path d="M240 192C240 147.8 275.8 112 320 112C364.2 112 400 147.8 400 192C400 236.2 364.2 272 320 272C275.8 272 240 236.2 240 192zM448 192C448 121.3 390.7 64 320 64C249.3 64 192 121.3 192 192C192 262.7 249.3 320 320 320C390.7 320 448 262.7 448 192zM144 544C144 473.3 201.3 416 272 416L368 416C438.7 416 496 473.3 496 544L496 552C496 565.3 506.7 576 520 576C533.3 576 544 565.3 544 552L544 544C544 446.8 465.2 368 368 368L272 368C174.8 368 96 446.8 96 544L96 552C96 565.3 106.7 576 120 576C133.3 576 144 565.3 144 552L144 544z"/>
                                    </svg>
                                </div>
                            </div>
                            <div class="card-body bg-light text-start p-4">
                                <p class="review-text m-0 text-dark">${(_b = op.comment) !== null && _b !== void 0 ? _b : ''}</p>
                            </div>
                        </div>
                    </div>`;
                    carouselInner.insertAdjacentHTML('beforeend', slideHtml);
                });
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