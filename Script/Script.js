
/* particles */
    (function(){
  const wrap = document.getElementById('particles');
    const n = 28;
    for(let i=0;i<n;i++){
    const p = document.createElement('span');
    p.className='particle';
    p.style.left = Math.random()*100+'%';
    p.style.top = Math.random()*100+'%';
    p.style.animationDelay = (Math.random()*6)+'s';
    p.style.animationDuration = (4+Math.random()*5)+'s';
    wrap.appendChild(p);
  }
})();

    /* scroll reveal */
    (function(){
  const els = document.querySelectorAll('.reveal');
  const io = new IntersectionObserver((entries)=>{
        entries.forEach(e => { if (e.isIntersecting) { e.target.classList.add('visible'); io.unobserve(e.target); } });
  }, {threshold:.15});
  els.forEach(el=>io.observe(el));
})();

    /* ---------- file handling ---------- */
    const fileGrid = document.getElementById('fileGrid');
    const fileCount = document.getElementById('fileCount');
    const dropzone = document.getElementById('dropzone');
    const fileInput = document.getElementById('fileInput');
    const browseLink = document.getElementById('browseLink');

    const TYPE_MAP = {
        pdf:{cls:'chip-pdf', icon:'fa-file-pdf'},
    zip:{cls:'chip-zip', icon:'fa-file-zipper'}, rar:{cls:'chip-zip', icon:'fa-file-zipper'},
    jpg:{cls:'chip-img', icon:'fa-file-image'}, jpeg:{cls:'chip-img', icon:'fa-file-image'}, png:{cls:'chip-img', icon:'fa-file-image'}, gif:{cls:'chip-img', icon:'fa-file-image'},
    mp4:{cls:'chip-vid', icon:'fa-file-video'}, mov:{cls:'chip-vid', icon:'fa-file-video'}
};
    function typeFor(name){
  const ext = name.split('.').pop().toLowerCase();
    return TYPE_MAP[ext] || {cls:'chip-img', icon:'fa-file'};
}
    function formatBytes(bytes){
  if(bytes < 1024) return bytes + ' B';
    if(bytes < 1024*1024) return (bytes/1024).toFixed(1)+' KB';
    if(bytes < 1024*1024*1024) return (bytes/(1024*1024)).toFixed(1)+' MB';
    return (bytes/(1024*1024*1024)).toFixed(1)+' GB';
}
    function updateCount(){
        fileCount.textContent = fileGrid.children.length;
}
    function addFileCard(name, sizeLabel){
  const t = typeFor(name);
    const col = document.createElement('div');
    col.className = 'col-md-6 col-lg-4';
    col.innerHTML = `
    <div class="glass file-card">
        <div class="file-icon-chip ${t.cls}"><i class="fa-solid ${t.icon}"></i></div>
        <div class="file-meta">
            <div class="file-name" title="${name}">${name}</div>
            <div class="file-size">${sizeLabel}</div>
        </div>
        <div class="remove-btn"><i class="fa-solid fa-xmark"></i></div>
    </div>`;
  col.querySelector('.remove-btn').addEventListener('click', ()=>{
        col.style.transition = 'opacity .25s, transform .25s';
    col.style.opacity='0'; col.style.transform='translateY(10px) scale(.95)';
    setTimeout(()=>{col.remove(); updateCount(); }, 220);
  });
    fileGrid.appendChild(col);
    updateCount();
}

    /* seed sample files so the section shows real content on load */
    addFileCard('presentation.pdf', '24.6 MB');
    addFileCard('vacation-photos.zip', '142 MB');
    addFileCard('demo-video.mp4', '89.2 MB');

    function handleFiles(fileList){
        Array.from(fileList).forEach(f => addFileCard(f.name, formatBytes(f.size)));
}
browseLink.addEventListener('click', ()=> fileInput.click());
dropzone.addEventListener('click', (e)=>{ if(e.target===dropzone || e.target.closest('.drop-icon')) fileInput.click(); });
fileInput.addEventListener('change', (e)=> handleFiles(e.target.files));

['dragenter','dragover'].forEach(evt=>{
        dropzone.addEventListener(evt, (e) => { e.preventDefault(); dropzone.classList.add('dragover'); });
});
['dragleave','drop'].forEach(evt=>{
        dropzone.addEventListener(evt, (e) => { e.preventDefault(); dropzone.classList.remove('dragover'); });
});
dropzone.addEventListener('drop', (e)=>{
  if(e.dataTransfer && e.dataTransfer.files.length) handleFiles(e.dataTransfer.files);
});

    /* ---------- progress simulation ---------- */
    (function(){
  const card = document.getElementById('progressCard');
    const fill = document.getElementById('progressFill');
    const pctEl = document.getElementById('progressPctValue');
    const speedEl = document.getElementById('progressSpeed');
    const timeEl = document.getElementById('progressTime');
    const statusEl = document.getElementById('progressStatus');
    const TOTAL_MB = 56;
    let pct = 0;

    function tick(){
    if(pct >= 100){
        card.classList.add('is-complete');
    pctEl.textContent = '100';
    fill.style.width = '100%';
    statusEl.textContent = 'Sent to MacBook Pro — Aman';
    timeEl.textContent = 'Done';
    speedEl.textContent = '—';
    clearInterval(timer);
      setTimeout(()=>{
        pct = 0; card.classList.remove('is-complete');
    statusEl.textContent = 'Sending to MacBook Pro — Aman';
    timer = setInterval(tick, 320);
      }, 3200);
    return;
    }
    pct += Math.random()*7 + 3;
    if(pct > 100) pct = 100;
    fill.style.width = pct + '%';
    pctEl.textContent = Math.round(pct);
    const speed = (12 + Math.random()*10);
    speedEl.textContent = speed.toFixed(1) + ' MB/s';
    const remainingMB = TOTAL_MB * (1 - pct/100);
    const secs = Math.max(1, Math.round(remainingMB / speed));
    timeEl.textContent = secs + 's';
  }
    let timer = setInterval(tick, 320);
})();

    /* ---------- fake QR pattern ---------- */
    (function(){
  const grid = document.getElementById('qrGrid');
    const pattern = [
    '010110010','101001011','001100100','110011001',
    '011010110','100101001','010110010','101001101','001101010'
    ];
  pattern.forEach(row=>{
        row.split('').forEach(bit => {
            const cell = document.createElement('div');
            cell.className = 'qr-cell' + (bit === '1' ? ' on' : '');
            grid.appendChild(cell);
        });
  });
})();

    /* ---------- QR card tilt ---------- */
    (function(){
  const card = document.getElementById('qrCard');
  card.addEventListener('mousemove', (e)=>{
    const r = card.getBoundingClientRect();
    const x = (e.clientX - r.left)/r.width - .5;
    const y = (e.clientY - r.top)/r.height - .5;
    card.style.transform = `rotateY(${x * 10}deg) rotateX(${-y * 10}deg) translateZ(0)`;
  });
  card.addEventListener('mouseleave', ()=>{card.style.transform = 'rotateY(0) rotateX(0)'; });
})();
