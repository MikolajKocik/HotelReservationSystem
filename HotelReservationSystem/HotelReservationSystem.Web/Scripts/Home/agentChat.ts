((): void => {
    const agentContainer = document.getElementById("agent-container");
    const agentButton: HTMLElement | null = document.getElementById("agent-button");
    const closeButton = document.querySelector(".agent-header .btn");

    agentButton?.addEventListener('click', (e) => {
        e.stopPropagation();

        agentContainer?.classList.toggle('hidden');
    })

    closeButton?.addEventListener('click', () => {
        agentContainer?.classList.add('hidden');
    });

    const sendMessage = async (text: string) => {
        const messagesList = document.querySelector(".text-messages-list");
        const token = (document.querySelector('input[name="__RequestVerificationToken"]') as HTMLInputElement)?.value;

        const userMsgHtml = `
        <div class="text-message p-2 mb-2 rounded ms-auto bg-primary text-white">
            ${text}
        </div>`;
        messagesList?.insertAdjacentHTML('beforeend', userMsgHtml);

        try {
            const response = await fetch('/Agent/Ask', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify({ message: text })
            });

            if (response.ok) {
                const data = await response.json();

                const botMsgHtml = `
                <div class="text-message p-2 mb-2 rounded me-auto border">
                    ${data.answer}
                </div>`;
                messagesList?.insertAdjacentHTML('beforeend', botMsgHtml);

                messagesList?.scrollTo(0, messagesList.scrollHeight);
            }
        } catch (error) {
            console.error("Kernel error:", error);
        }
    }
})();