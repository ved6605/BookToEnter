let html5QrScanner = null;

function openScanner(){
    document.getElementById('scannerModal').style.display = 'flex';
    document.getElementById('scannerModal').setAttribute('aria-hidden','false');
    startScanner();
}

function closeScanner(){
    stopScanner();
    document.getElementById('scannerModal').style.display = 'none';
    document.getElementById('scannerModal').setAttribute('aria-hidden','true');
}

function startScanner(){
    if (html5QrScanner) return;
    const qrRegionId = 'qr-reader';
    html5QrScanner = new Html5Qrcode(qrRegionId);
    const config = { fps: 10, qrbox: { width: 300, height: 300 } };

    html5QrScanner.start(
        { facingMode: { exact: "environment" } },
        config,
        qrMessage => {
            // stop immediately on detection
            stopScanner();
            // show quick animation (can be tweaked)
            flashResult();
            // send to server
            fetch('/Coordinator/ValidateQr', { method: 'POST', headers: { 'Content-Type': 'application/x-www-form-urlencoded' }, body: `qrData=${encodeURIComponent(qrMessage)}` })
                .then(r => r.json())
                .then(data => {
                    if (data?.success) showScanDetails(data.student);
                    else showScanDetails({error: data?.error || 'Invalid QR'});
                })
                .catch(err => showScanDetails({error: 'Server error'}));
        },
        errorMessage => {
            // console.debug('QR scan error', errorMessage);
        }
    ).catch(err => {
        alert('Unable to start camera. Make sure the site has camera permission and that you are using a secure context (https).');
        console.error(err);
    });
}

function stopScanner(){
    if (!html5QrScanner) return;
    html5QrScanner.stop().then(() => {
        html5QrScanner.clear();
        html5QrScanner = null;
    }).catch(err => {
        console.error('Failed to stop scanner', err);
        html5QrScanner = null;
    });
}

function showScanDetails(student){
    const box = document.getElementById('scanDetails');
    if (student.error){
        box.innerHTML = `<div class="booking-card"><strong class="muted">Error:</strong> ${student.error}</div>`;
        return;
    }
    box.innerHTML = `
        <div class="booking-card">
            <div style="display:flex;gap:12px;align-items:center;">
                <img src="${student.idImageUrl}" alt="ID" style="width:84px;height:84px;border-radius:8px;border:1px solid rgba(255,255,255,.06);object-fit:cover;" />
                <div>
                    <h4 style="margin:0">${student.name}</h4>
                    <div style="color:var(--muted);">${student.enrollment}</div>
                    <div style="color:var(--muted);font-size:0.9rem">${student.email}</div>
                </div>
            </div>
            <div style="margin-top:10px;display:flex;gap:8px;">
                <button class="btn-primary" onclick="confirmEntry('${student.enrollment}')">Allow Entry</button>
                <button class="btn-secondary" onclick="rejectEntry('${student.enrollment}')">Deny</button>
            </div>
        </div>
    `;
}

function confirmEntry(enrollment){
    alert('Entry confirmed for ' + enrollment + '. (Hook this to server in production)');
}

function rejectEntry(enrollment){
    alert('Entry denied for ' + enrollment + '. (Hook this to server in production)');
}

function flashResult(){
    const el = document.querySelector('.scanner-modal');
    if (!el) return;
    el.animate([{ boxShadow: '0 0 0 0 rgba(0,224,184,0.0)' }, { boxShadow: '0 0 24px 8px rgba(0,224,184,0.18)' }], { duration: 420, easing: 'ease-out' });
}