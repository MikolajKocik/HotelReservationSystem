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
    const getBootstrap = () => {
        var _a;
        const w = window;
        return (_a = w.bootstrap) !== null && _a !== void 0 ? _a : null;
    };
    const hideOtherOpenModals = (exceptId) => {
        const bs = getBootstrap();
        if (!(bs === null || bs === void 0 ? void 0 : bs.Modal))
            return;
        const openModals = Array.from(document.querySelectorAll('.modal.show'));
        for (const modal of openModals) {
            if (modal.id === exceptId)
                continue;
            bs.Modal.getOrCreateInstance(modal).hide();
        }
    };
    const showModalById = (id) => {
        const modal = document.getElementById(id);
        const bs = getBootstrap();
        if (!modal || !(bs === null || bs === void 0 ? void 0 : bs.Modal))
            return;
        bs.Modal.getOrCreateInstance(modal).show();
    };
    const getReturnUrlInput = () => {
        var _a;
        return ((_a = document.querySelector('#loginModal #loginReturnUrl')) !== null && _a !== void 0 ? _a : document.getElementById('loginReturnUrl'));
    };
    const setReturnUrlIfEmpty = () => {
        const returnUrlInput = getReturnUrlInput();
        if (!returnUrlInput)
            return;
        if (returnUrlInput.value && returnUrlInput.value.trim().length > 0)
            return;
        returnUrlInput.value = window.location.pathname + window.location.search + window.location.hash;
    };
    const wireLoginForm = () => {
        var _a;
        const form = (_a = document.querySelector('#loginModal #loginForm')) !== null && _a !== void 0 ? _a : document.getElementById('loginForm');
        if (!form)
            return;
        if (form.dataset.wired === 'true')
            return;
        form.dataset.wired = 'true';
        form.addEventListener('submit', (event) => __awaiter(void 0, void 0, void 0, function* () {
            var _a;
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
                form.classList.add('was-validated');
                return;
            }
            form.classList.add('was-validated');
            setReturnUrlIfEmpty();
            const isInModal = !!form.closest('#loginModal');
            if (!isInModal)
                return; // full-page login should submit normally
            event.preventDefault();
            event.stopPropagation();
            try {
                const response = yield fetch(form.action, {
                    method: 'POST',
                    body: new FormData(form),
                    headers: {
                        'X-Requested-With': 'XMLHttpRequest',
                        'Accept': 'application/json'
                    }
                });
                const contentType = (_a = response.headers.get('content-type')) !== null && _a !== void 0 ? _a : '';
                if (response.ok && contentType.includes('application/json')) {
                    const data = yield response.json();
                    if (data === null || data === void 0 ? void 0 : data.redirectUrl) {
                        window.location.assign(data.redirectUrl);
                    }
                    else {
                        window.location.reload();
                    }
                    return;
                }
                const html = yield response.text();
                const modalBody = document.getElementById('loginModalBody');
                if (modalBody) {
                    modalBody.innerHTML = html;
                    wireLoginForm();
                    setReturnUrlIfEmpty();
                }
            }
            catch (_b) {
            }
        }));
    };
    document.addEventListener('click', (event) => {
        const target = event.target;
        if (!target)
            return;
        const loginTrigger = target.closest('[data-bs-target="#loginModal"], a[href="#loginModal"]');
        if (loginTrigger) {
            event.preventDefault();
            hideOtherOpenModals('loginModal');
            showModalById('loginModal');
            return;
        }
        const registerTrigger = target.closest('[data-bs-target="#registerModal"], a[href="#registerModal"]');
        if (registerTrigger) {
            event.preventDefault();
            hideOtherOpenModals('registerModal');
            showModalById('registerModal');
            return;
        }
    }, true);
    document.addEventListener('shown.bs.modal', (event) => {
        var _a;
        const modal = event.target;
        if (((_a = modal === null || modal === void 0 ? void 0 : modal.id) !== null && _a !== void 0 ? _a : '') === 'loginModal') {
            setReturnUrlIfEmpty();
            wireLoginForm();
        }
    });
    setReturnUrlIfEmpty();
    wireLoginForm();
})();
//# sourceMappingURL=loginForm.js.map