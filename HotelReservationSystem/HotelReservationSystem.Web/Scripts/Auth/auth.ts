((): void => {
    const getModalInstance = (id: string) => {
        const el = document.getElementById(id);
        const bootstrap = (window as any).bootstrap;
        return el && bootstrap 
            ? bootstrap.Modal.getOrCreateInstance(el) 
            : null;
    }

    const handleFormSubmit = async (form: HTMLFormElement, modalBodyId: string, reWire: () => void) => {
        if (!form.checkValidity()) {
            form.classList.add('was-validated');
            return;
        }

        try {
            const response = await fetch(form.action, {
                method: 'POST',
                body: new FormData(form),
                headers: { 'X-Requested-With': 'XMLHttpRequest' }
            });

            const contentType = response.headers.get('Content-Type') || '';

            if (response.ok && contentType.includes('application/json')) {
                const data = await response.json();
                window.location.assign(data.redirectUrl || '/');
                return;
            }

            const html = await response.text();
            const modalBody = document.getElementById(modalBodyId);
            if (modalBody) {
                modalBody.innerHTML = html;
                reWire();
            }
        } catch (error) {
            console.error('Auth error:', error);
        }
    }

    const wireLoginForm = (): void => {
        const form = document.getElementById('loginForm') as HTMLFormElement;
        if (!form || form.dataset.wired) return;
        form.dataset.wired = 'true';
        form.addEventListener('submit', (e: Event) => {
            e.preventDefault();
            handleFormSubmit(form, 'loginModalBody', wireLoginForm);
        });
    }

    const wireRegisterForm = (): void => {
        const form = document.getElementById('registerForm') as HTMLFormElement;
        if (!form || form.dataset.wired) return;
        form.dataset.wired = 'true';
        form.addEventListener('submit', (e: Event) => {
            e.preventDefault();
            handleFormSubmit(form, 'registerModalBody', wireRegisterForm);
        });
    }

    document.addEventListener('click', (e) => {
        const target = e.target as HTMLElement;
        if (target.dataset.bsTarget === '#registerModal') {
            getModalInstance('loginModal')?.hide();
        }

        if (target.dataset.bsTarget === '#loginModal') {
            getModalInstance('registerModal')?.hide();
        }
    });

    document.addEventListener('DOMContentLoaded', () => {
        wireLoginForm();
        wireRegisterForm();
    });
})();