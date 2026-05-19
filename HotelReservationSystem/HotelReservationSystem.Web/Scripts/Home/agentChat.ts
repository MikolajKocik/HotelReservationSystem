interface UserRequest {
    message: string,
    sessionId: string
}

((): void => {
    const agentContainer = document.getElementById("agent-container");
    const agentButton: HTMLElement | null = document.getElementById("agent-button");
    const agentChatButton = document.getElementById("agent-chat-button");
    const closeButton = document.querySelector(".agent-header .btn-close-chat");
    const messagesList = document.querySelector(".text-messages-list");
    const typingIndicator = document.getElementById("chat-typing-indicator");

    let sessionId = sessionStorage.getItem("SESSION_ID");

    if (!sessionId) {
        sessionId = crypto.randomUUID();
        sessionStorage.setItem("SESSION_ID", sessionId);
    }

    agentButton?.addEventListener('click', (e) => {
        e.stopPropagation();
        agentContainer?.classList.toggle('d-none');
        if (messagesList) {
            messagesList.scrollTo(0, messagesList.scrollHeight);
        }
    });

    agentChatButton?.addEventListener('click', (e) => {
        e.stopPropagation();
        agentContainer?.classList.toggle('d-none');
        if(messagesList) {
            messagesList.scrollTo(0, messagesList.scrollHeight);
        }
    });

    closeButton?.addEventListener('click', () => {
        agentContainer?.classList.add('d-none');
        if (messagesList) {
            messagesList.innerHTML = '';
            sessionStorage.removeItem("SESSION_ID");
            sessionId = crypto.randomUUID();
            sessionStorage.setItem("SESSION_ID", sessionId);
        }
    });

    document.addEventListener('click', function(event) {
        if (!(event.target instanceof Node)) return;
        if (!agentContainer || !agentChatButton) return;

        const isOpen = !agentContainer.classList.contains('d-none');

        if (isOpen) {
            const clickedOutsideContainer = !agentContainer.contains(event.target);            
            const clickedOutsideButton = !agentChatButton.contains(event.target);

            if (clickedOutsideContainer && clickedOutsideButton) {
                agentContainer.classList.add('d-none');
            }
        }
    });

    const sendMessage = async (request: UserRequest) => {
        const token = (document.querySelector('input[name="__RequestVerificationToken"]') as HTMLInputElement)?.value;

        const userMsgHtml = `
        <div class="text-message p-2 px-3 shadow-sm user-message align-self-end text-white bg-primary">
            ${request.message}
        </div>`;
        messagesList?.insertAdjacentHTML('beforeend', userMsgHtml);
        if (messagesList) {
            messagesList.scrollTo(0, messagesList.scrollHeight);
        }

        // Show typing indicator
        typingIndicator?.classList.remove('d-none');
        if (messagesList) {
            messagesList.scrollTo(0, messagesList.scrollHeight);
        }

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
                <div class="text-message p-2 px-3 shadow-sm bot-message align-self-start bg-white text-dark border">
                    ${data.answer}
                </div>`;
                messagesList?.insertAdjacentHTML('beforeend', botMsgHtml);
            } else {
                const botMsgHtml = `
                <div class="text-message p-2 px-3 shadow-sm bot-message align-self-start bg-white text-danger border">
                    Przepraszam, wystąpił problem podczas komunikacji z serwerem.
                </div>`;
                messagesList?.insertAdjacentHTML('beforeend', botMsgHtml);
            }
        } catch (error) {
            console.error("Kernel error:", error);
            const botMsgHtml = `
            <div class="text-message p-2 px-3 shadow-sm bot-message align-self-start bg-white text-danger border">
                Przepraszam, wystąpił błąd sieci.
            </div>`;
            messagesList?.insertAdjacentHTML('beforeend', botMsgHtml);
        } finally {
            // Hide typing indicator
            typingIndicator?.classList.add('d-none');
            if (messagesList) {
                messagesList.scrollTo(0, messagesList.scrollHeight);
            }
        }
    };

    const messageInput = document.querySelector(".chat-input") as HTMLTextAreaElement;
    const sendButton = document.getElementById("send-btn");

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
            // Reset height if resized
            messageInput.style.height = 'auto';
        }
    }

    sendButton?.addEventListener('click', handleSend);

    messageInput?.addEventListener('keydown', (e) => {
        if (e.key === 'Enter' && !e.shiftKey) {
            e.preventDefault();
            handleSend(e);
        }
    });

    // Auto-resize input textarea based on content
    messageInput?.addEventListener('input', () => {
        messageInput.style.height = 'auto';
        messageInput.style.height = (messageInput.scrollHeight) + 'px';
    });
})();