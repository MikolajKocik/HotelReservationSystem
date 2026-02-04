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
    'use strict';
    const openModalFromHash = () => {
        const hash = window.location.hash;
        const modalMap = {
            '#createSingleModal': 'createSingleModal',
            '#createDoubleModal': 'createDoubleModal'
        };
        const modalId = modalMap[hash];
        if (!modalId)
            return;
        const modalElement = document.getElementById(modalId);
        if (!modalElement)
            return;
        const bootstrap = window.bootstrap;
        if (!(bootstrap === null || bootstrap === void 0 ? void 0 : bootstrap.Modal))
            return;
        bootstrap.Modal.getOrCreateInstance(modalElement).show();
    };
    const initRoomDetailsModals = () => {
        document.querySelectorAll('[data-room-details-id]').forEach(button => {
            button.addEventListener('click', () => __awaiter(void 0, void 0, void 0, function* () {
                const roomId = button.dataset.roomDetailsId;
                if (!roomId)
                    return;
                const modalContainer = document.getElementById('roomDetailsModalContainer');
                if (!modalContainer)
                    return;
                try {
                    const response = yield fetch(`/Room/DetailsPartial/${roomId}`);
                    if (!response.ok)
                        throw new Error('Failed to load room details');
                    const html = yield response.text();
                    modalContainer.innerHTML = html;
                    const modalElement = modalContainer.querySelector('.modal');
                    if (modalElement) {
                        const bootstrap = window.bootstrap;
                        if (bootstrap === null || bootstrap === void 0 ? void 0 : bootstrap.Modal) {
                            bootstrap.Modal.getOrCreateInstance(modalElement).show();
                        }
                    }
                }
                catch (error) {
                    console.error('Error loading room details:', error);
                }
            }));
        });
    };
    document.addEventListener('DOMContentLoaded', () => {
        openModalFromHash();
        initRoomDetailsModals();
    });
    if (document.readyState !== 'loading') {
        openModalFromHash();
        initRoomDetailsModals();
    }
})();
//# sourceMappingURL=room.js.map