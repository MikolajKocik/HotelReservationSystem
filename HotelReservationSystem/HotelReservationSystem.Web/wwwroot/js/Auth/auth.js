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
    const getModalInstance = (id) => {
        const el = document.getElementById(id);
        const bootstrap = window.bootstrap;
        return el && bootstrap
            ? bootstrap.Modal.getOrCreateInstance(el)
            : null;
    };
    const handleFormSubmit = (form, modalBodyId, reWire) => __awaiter(void 0, void 0, void 0, function* () {
        if (!form.checkValidity()) {
            form.classList.add('was-validated');
            return;
        }
        try {
            const response = yield fetch(form.action, {
                method: 'POST',
                body: new FormData(form),
                headers: { 'X-Requested-With': 'XMLHttpRequest' }
            });
            const contentType = response.headers.get('Content-Type') || '';
            if (response.ok && contentType.includes('application/json')) {
                const data = yield response.json();
                window.location.assign(data.redirectUrl || '/');
                return;
            }
            const html = yield response.text();
            const modalBody = document.getElementById(modalBodyId);
            if (modalBody) {
                modalBody.innerHTML = html;
                reWire();
            }
        }
        catch (error) {
            console.error('Auth error:', error);
        }
    });
    const wireLoginForm = () => {
        const form = document.getElementById('loginForm');
        if (!form || form.dataset.wired)
            return;
        form.dataset.wired = 'true';
        form.addEventListener('submit', (e) => {
            e.preventDefault();
            handleFormSubmit(form, 'loginModalBody', wireLoginForm);
        });
    };
    const wireRegisterForm = () => {
        const form = document.getElementById('registerForm');
        if (!form || form.dataset.wired)
            return;
        form.dataset.wired = 'true';
        form.addEventListener('submit', (e) => {
            e.preventDefault();
            handleFormSubmit(form, 'registerModalBody', wireRegisterForm);
        });
    };
    document.addEventListener('click', (e) => {
        var _a, _b;
        const target = e.target;
        if (target.dataset.bsTarget === '#registerModal') {
            (_a = getModalInstance('loginModal')) === null || _a === void 0 ? void 0 : _a.hide();
        }
        if (target.dataset.bsTarget === '#loginModal') {
            (_b = getModalInstance('registerModal')) === null || _b === void 0 ? void 0 : _b.hide();
        }
    });
    document.addEventListener('DOMContentLoaded', () => {
        wireLoginForm();
        wireRegisterForm();
    });
})();
//# sourceMappingURL=auth.js.map