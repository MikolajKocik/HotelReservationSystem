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
    const agentContainer = document.getElementById("agent-container");
    const agentButton = document.getElementById("agent-button");
    const closeButton = document.querySelector(".agent-header .btn");
    agentButton === null || agentButton === void 0 ? void 0 : agentButton.addEventListener('click', (e) => {
        e.stopPropagation();
        agentContainer === null || agentContainer === void 0 ? void 0 : agentContainer.classList.toggle('hidden');
    });
    closeButton === null || closeButton === void 0 ? void 0 : closeButton.addEventListener('click', () => {
        agentContainer === null || agentContainer === void 0 ? void 0 : agentContainer.classList.add('hidden');
        const messagesList = document.querySelector(".text-messages-list");
        if (messagesList) {
            messagesList.innerHTML = '';
        }
    });
    const sendMessage = (text) => __awaiter(void 0, void 0, void 0, function* () {
        var _a;
        const messagesList = document.querySelector(".text-messages-list");
        const token = (_a = document.querySelector('input[name="__RequestVerificationToken"]')) === null || _a === void 0 ? void 0 : _a.value;
        const userMsgHtml = `
        <div class="text-message p-2 mb-2 rounded ms-auto bg-primary text-white" style="max-width: 70%;">
            ${text}
        </div>`;
        messagesList === null || messagesList === void 0 ? void 0 : messagesList.insertAdjacentHTML('beforeend', userMsgHtml);
        try {
            const response = yield fetch('/Agent/Ask', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify({ message: text })
            });
            if (response.ok) {
                const data = yield response.json();
                const botMsgHtml = `
                <div class="text-message p-2 mb-2 rounded me-auto border" style="max-width: 70%;">
                    ${data.answer}
                </div>`;
                messagesList === null || messagesList === void 0 ? void 0 : messagesList.insertAdjacentHTML('beforeend', botMsgHtml);
                messagesList === null || messagesList === void 0 ? void 0 : messagesList.scrollTo(0, messagesList.scrollHeight);
            }
        }
        catch (error) {
            console.error("Kernel error:", error);
        }
    });
    const messageInput = document.querySelector(".send-message .text-wrapper textarea");
    const sendButton = document.querySelector(".send-message .btn-message-wrapper a");
    sendButton === null || sendButton === void 0 ? void 0 : sendButton.addEventListener('click', (e) => {
        e.preventDefault();
        const text = messageInput === null || messageInput === void 0 ? void 0 : messageInput.value.trim();
        if (text) {
            sendMessage(text);
            messageInput.value = '';
        }
    });
    messageInput === null || messageInput === void 0 ? void 0 : messageInput.addEventListener('keydown', (e) => {
        if (e.key === 'Enter' && !e.shiftKey) {
            e.preventDefault();
            const text = messageInput.value.trim();
            if (text) {
                sendMessage(text);
                messageInput.value = '';
            }
        }
    });
})();
//# sourceMappingURL=agentChat.js.map