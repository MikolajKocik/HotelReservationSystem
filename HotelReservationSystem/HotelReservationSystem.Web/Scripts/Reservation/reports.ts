(function(){
    async function exportCsv(data){
        const rows = [Object.keys(data[0])].concat(data.map(r=>Object.values(r)));
        const csv = rows.map(r=>r.map(v=>`"${String(v).replace(/"/g,'""')}"`).join(',')).join('\n');
        const blob = new Blob([csv],{type:'text/csv'});
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a'); a.href = url; a.download='report.csv'; a.click(); URL.revokeObjectURL(url);
    }

    async function exportPdf(text){
        // basic PDF via blob (server-side PDF generation preferred)
        const blob = new Blob([text],{type:'application/pdf'});
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a'); a.href = url; a.download='report.pdf'; a.click(); URL.revokeObjectURL(url);
    }

    async function exportXlsx(data){
        // produce simple CSV with .xlsx extension (for demo). For real xlsx use a library like SheetJS.
        await exportCsv(data);
    }

    async function uploadSharepoint(){
        try{
            const res = await fetch('/api/reports/upload');
            if(!res.ok) alert('Błąd wysyłania do SharePoint');
            else alert('Wysłano do SharePoint/Punkt docelowy');
        }catch(e){console.error(e); alert('Błąd sieci');}
    }

    document.addEventListener('DOMContentLoaded', ()=>{
        const btnCsv = document.getElementById('export-csv');
        const btnPdf = document.getElementById('export-pdf');
        const btnXlsx = document.getElementById('export-excel');
        const btnUpload = document.getElementById('upload-sharepoint');
        const preview = document.querySelector('#report-preview pre');
        if(!preview) return;
        let data: any = [];
        try{ data = JSON.parse(preview.textContent || '[]'); }catch(e){ data = [{message: preview.textContent || ''}]; }
        if(btnCsv) btnCsv.addEventListener('click', ()=>exportCsv(data));
        if(btnPdf) btnPdf.addEventListener('click', ()=>exportPdf(typeof data==='string'?data:String(JSON.stringify(data, null, 2))));
        if(btnXlsx) btnXlsx.addEventListener('click', ()=>exportXlsx(data));
        if(btnUpload) btnUpload.addEventListener('click', ()=>uploadSharepoint());
    });
})();
