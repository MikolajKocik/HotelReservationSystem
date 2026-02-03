((): void => {
    'use strict';

    type BootstrapApi = {
        Modal?: {
            getInstance: (el: Element) => { hide: () => void } | null;
            getOrCreateInstance: (el: Element) => { show: () => void; hide: () => void };
        };
    };

    const getBootstrap = (): BootstrapApi | null => {
        const w = window as unknown as { bootstrap?: BootstrapApi };
        return w.bootstrap ?? null;
    };

    const hideOtherOpenModals = (exceptId: string): void => {
        const bs = getBootstrap();
        if (!bs?.Modal) return;

        const openModals = Array.from(document.querySelectorAll<HTMLElement>('.modal.show'));
        for (const modal of openModals) {
            if (modal.id === exceptId) continue;
            bs.Modal.getOrCreateInstance(modal).hide();
        }
    };

    const showModalById = (id: string): void => {
        const modal = document.getElementById(id);
        const bs = getBootstrap();
        if (!modal || !bs?.Modal) return;
        bs.Modal.getOrCreateInstance(modal).show();
    };

    const getReturnUrlInput = (): HTMLInputElement | null => {
        return (
            document.querySelector<HTMLInputElement>('#loginModal #loginReturnUrl') ??
            document.getElementById('loginReturnUrl') as HTMLInputElement | null
        );
    };

    const setReturnUrlIfEmpty = (): void => {
        const returnUrlInput = getReturnUrlInput();
        if (!returnUrlInput) return;
        if (returnUrlInput.value && returnUrlInput.value.trim().length > 0) return;
        returnUrlInput.value = window.location.pathname + window.location.search + window.location.hash;
    };

    const wireLoginForm = (): void => {
        const form = document.querySelector<HTMLFormElement>('#loginModal #loginForm') ??
            document.getElementById('loginForm') as HTMLFormElement | null;
        if (!form) return;

        if (form.dataset.wired === 'true') return;
        form.dataset.wired = 'true';

        form.addEventListener('submit', async (event: Event) => {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
                form.classList.add('was-validated');
                return;
            }

            form.classList.add('was-validated');
            setReturnUrlIfEmpty();

            const isInModal = !!form.closest('#loginModal');
            if (!isInModal) return; // full-page login should submit normally

            event.preventDefault();
            event.stopPropagation();

            try {
                const response = await fetch(form.action, {
                    method: 'POST',
                    body: new FormData(form),
                    headers: {
                        'X-Requested-With': 'XMLHttpRequest',
                        'Accept': 'application/json'
                    }
                });

                const contentType = response.headers.get('content-type') ?? '';
                if (response.ok && contentType.includes('application/json')) {
                    const data = await response.json() as { redirectUrl?: string };
                    if (data?.redirectUrl) {
                        window.location.assign(data.redirectUrl);
                    } else {
                        window.location.reload();
                    }
                    return;
                }

                const html = await response.text();
                const modalBody = document.getElementById('loginModalBody');
                if (modalBody) {
                    modalBody.innerHTML = html;
                    wireLoginForm();
                    setReturnUrlIfEmpty();
                }
            } catch {
            }
        });
    };

    document.addEventListener('click', (event: MouseEvent) => {
        const target = event.target as HTMLElement | null;
        if (!target) return;

        const loginTrigger = target.closest<HTMLElement>('[data-bs-target="#loginModal"], a[href="#loginModal"]');
        if (loginTrigger) {
            event.preventDefault();
            hideOtherOpenModals('loginModal');
            showModalById('loginModal');
            return;
        }

        const registerTrigger = target.closest<HTMLElement>('[data-bs-target="#registerModal"], a[href="#registerModal"]');
        if (registerTrigger) {
            event.preventDefault();
            hideOtherOpenModals('registerModal');
            showModalById('registerModal');
            return;
        }
    }, true);

    document.addEventListener('shown.bs.modal', (event: Event) => {
        const modal = event.target as HTMLElement | null;
        if ((modal?.id ?? '') === 'loginModal') {
            setReturnUrlIfEmpty();
            wireLoginForm();
        }
    });

    setReturnUrlIfEmpty();
    wireLoginForm();
})();