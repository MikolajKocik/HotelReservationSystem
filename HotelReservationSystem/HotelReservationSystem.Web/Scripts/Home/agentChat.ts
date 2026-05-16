interface UserRequest {
    message: string,
    sessionId: string 
}

((): void => {
    const agentContainer = document.getElementById("agent-container");
    const agentButton: HTMLElement | null = document.getElementById("agent-button");
    const closeButton = document.querySelector(".agent-header .btn");
    const messagesList = document.querySelector(".text-messages-list");

    let sessionId = sessionStorage.getItem("SESSION_ID");

    if (!sessionId) {
        sessionId = crypto.randomUUID(); 
        sessionStorage.setItem("SESSION_ID", sessionId);
    }

    agentButton?.addEventListener('click', (e) => {
        e.stopPropagation();
        agentContainer?.classList.toggle('hidden');
    });

    closeButton?.addEventListener('click', () => {
        agentContainer?.classList.add('hidden');
        if (messagesList) {
            messagesList.innerHTML = '';
            sessionStorage.removeItem("SESSION_ID");
            sessionId = crypto.randomUUID();
            sessionStorage.setItem("SESSION_ID", sessionId);
        }       
    });

    const sendMessage = async (request: UserRequest) => {
        const token = (document.querySelector('input[name="__RequestVerificationToken"]') as HTMLInputElement)?.value;

        const userMsgHtml = `
        <div class="text-message p-2 mb-2 rounded ms-auto bg-primary text-white" style="max-width: 70%;">
            ${request.message}
        </div>`;
        messagesList?.insertAdjacentHTML('beforeend', userMsgHtml);

        try {
            const response = await fetch('/Agent/Ask', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify(request)
            });

            if (response.ok) {
                const data = await response.json();

                const botMsgHtml = `
                <div class="text-message p-2 mb-2 rounded me-auto border" style="max-width: 70%;">
                    ${data.answer}
                </div>`;
                messagesList?.insertAdjacentHTML('beforeend', botMsgHtml);

                messagesList?.scrollTo(0, messagesList.scrollHeight);
            }
        } catch (error) {
            console.error("Kernel error:", error);
        }
    };

    const messageInput = document.querySelector(".send-message .text-wrapper textarea") as HTMLTextAreaElement;
    const sendButton = document.querySelector(".send-message .btn-message-wrapper a");

    const handleSend = (e?: Event) => {
        e?.stopPropagation();
        const text = messageInput?.value.trim();

        if (text && sessionId) {
            const payload: UserRequest = {
                message: text,
                sessionId: sessionId
            }
            sendMessage(payload);
            messageInput.value = '';
        }
    }

    sendButton?.addEventListener('click', handleSend);

    messageInput?.addEventListener('keydown', (e) => {
        if (e.key === 'Enter' && !e.shiftKey) {
            handleSend(e);
        }
    });
})();