((): void => {
    'use strict';
    const openModalFromHash = (): void => {
        const hash = window.location.hash;
        
        const modalMap: Record<string, string> = {
            '#createSingleModal': 'createSingleModal',
            '#createDoubleModal': 'createDoubleModal'
        };

        const modalId = modalMap[hash];
        if (!modalId) return;

        const modalElement = document.getElementById(modalId);
        if (!modalElement) return;

        const bootstrap = (window as any).bootstrap;
        if (!bootstrap?.Modal) return;

        bootstrap.Modal.getOrCreateInstance(modalElement).show();
    };

    const initRoomDetailsModals = (): void => {
        document.querySelectorAll<HTMLButtonElement>('[data-room-details-id]').forEach(button => {
            button.addEventListener('click', async () => {
                const roomId = button.dataset.roomDetailsId;
                if (!roomId) return;

                const modalContainer = document.getElementById('roomDetailsModalContainer');
                if (!modalContainer) return;

                try {
                    const response = await fetch(`/Room/DetailsPartial/${roomId}`);
                    if (!response.ok) throw new Error('Failed to load room details');
                    
                    const html = await response.text();
                    modalContainer.innerHTML = html;

                    const modalElement = modalContainer.querySelector('.modal');
                    if (modalElement) {
                        const bootstrap = (window as any).bootstrap;
                        if (bootstrap?.Modal) {
                            bootstrap.Modal.getOrCreateInstance(modalElement).show();
                        }
                    }
                } catch (error) {
                    console.error('Error loading room details:', error);
                }
            });
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
