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
(function () {
    function exportCsv(data) {
        return __awaiter(this, void 0, void 0, function* () {
            const rows = [Object.keys(data[0])].concat(data.map(r => Object.values(r)));
            const csv = rows.map(r => r.map(v => `"${String(v).replace(/"/g, '""')}"`).join(',')).join('\n');
            const blob = new Blob([csv], { type: 'text/csv' });
            const url = URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = 'report.csv';
            a.click();
            URL.revokeObjectURL(url);
        });
    }
    function exportPdf(text) {
        return __awaiter(this, void 0, void 0, function* () {
            // basic PDF via blob (server-side PDF generation preferred)
            const blob = new Blob([text], { type: 'application/pdf' });
            const url = URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = 'report.pdf';
            a.click();
            URL.revokeObjectURL(url);
        });
    }
    function exportXlsx(data) {
        return __awaiter(this, void 0, void 0, function* () {
            // produce simple CSV with .xlsx extension (for demo). For real xlsx use a library like SheetJS.
            yield exportCsv(data);
        });
    }
    function uploadSharepoint() {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                const res = yield fetch('/api/reports/upload');
                if (!res.ok)
                    alert('Błąd wysyłania do SharePoint');
                else
                    alert('Wysłano do SharePoint/Punkt docelowy');
            }
            catch (e) {
                console.error(e);
                alert('Błąd sieci');
            }
        });
    }
    document.addEventListener('DOMContentLoaded', () => {
        const btnCsv = document.getElementById('export-csv');
        const btnPdf = document.getElementById('export-pdf');
        const btnXlsx = document.getElementById('export-excel');
        const btnUpload = document.getElementById('upload-sharepoint');
        const preview = document.querySelector('#report-preview pre');
        if (!preview)
            return;
        let data = [];
        try {
            data = JSON.parse(preview.textContent || '[]');
        }
        catch (e) {
            data = [{ message: preview.textContent || '' }];
        }
        if (btnCsv)
            btnCsv.addEventListener('click', () => exportCsv(data));
        if (btnPdf)
            btnPdf.addEventListener('click', () => exportPdf(typeof data === 'string' ? data : String(JSON.stringify(data, null, 2))));
        if (btnXlsx)
            btnXlsx.addEventListener('click', () => exportXlsx(data));
        if (btnUpload)
            btnUpload.addEventListener('click', () => uploadSharepoint());
    });
})();
//# sourceMappingURL=reports.js.map