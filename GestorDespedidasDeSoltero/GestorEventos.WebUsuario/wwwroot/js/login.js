// Modal
function showModal(modalId) {
    document.getElementById(modalId).style.display = 'block';
}

function closeModal(modalId) {
    const modal = document.getElementById(modalId);
    modal.style.animation = 'fadeOut 0.3s ease-out forwards, slideOut 0.3s ease-out forwards';
    setTimeout(() => {
        modal.style.display = 'none';
        modal.style.animation = '';
    }, 300);
}

function setupModalEvents() {
    const modals = document.querySelectorAll('.modal');

    window.addEventListener('click', function (event) {
        modals.forEach(modal => {
            if (event.target === modal) {
                closeModal(modal.id);
            }
        });
    });

    window.addEventListener('keydown', function (event) {
        if (event.key === 'Escape') {
            modals.forEach(modal => {
                if (modal.style.display === 'block') {
                    closeModal(modal.id);
                }
            });
        }
    });
}
setupModalEvents();

const items = document.querySelectorAll('.carousel-item');
const indicators = document.querySelectorAll('.carousel-indicators span');
let currentItem = 0;

function showItem(index) {
    items[currentItem].classList.remove('active');
    indicators[currentItem].classList.remove('active');
    currentItem = index;
    items[currentItem].classList.add('active');
    indicators[currentItem].classList.add('active');
}

indicators.forEach((indicator, index) => {
    indicator.addEventListener('click', () => {
        showItem(index);
    });
});

setInterval(() => {
    let nextItem = (currentItem + 1) % items.length;
    showItem(nextItem);
}, 6000);

let isDragging = false;
let startPos = 0;
let currentTranslate = 0;
let prevTranslate = 0;

const leftPanel = document.querySelector('.left');
const carouselItems = document.querySelectorAll('.carousel-item');

leftPanel.addEventListener('mousedown', dragStart);
leftPanel.addEventListener('mouseup', dragEnd);
leftPanel.addEventListener('mousemove', dragAction);
leftPanel.addEventListener('mouseleave', dragEnd);

function dragStart(event) {
    isDragging = true;
    startPos = event.clientX;
    leftPanel.classList.add('dragging');
}

function dragEnd() {
    isDragging = false;
    leftPanel.classList.remove('dragging');
    prevTranslate = currentTranslate;
}

function dragAction(event) {
    if (!isDragging) return;
    const currentPos = event.clientX;
    const diff = currentPos - startPos;

    if (Math.abs(diff) > 100) { // Threshold to change slide
        if (diff > 0) {
            showPrevItem();
        } else {
            showNextItem();
        }
        dragEnd();
    }
}

function showNextItem() {
    let nextItem = (currentItem + 1) % items.length;
    showItem(nextItem);
}

function showPrevItem() {
    let prevItem = (currentItem - 1 + items.length) % items.length;
    showItem(prevItem);
}

function showItem(index) {
    items[currentItem].classList.remove('active');
    indicators[currentItem].classList.remove('active');
    currentItem = index;
    items[currentItem].classList.add('active');
    indicators[currentItem].classList.add('active');
}

const notification = document.getElementById('logoutNotification');
const closeBtn = notification.querySelector('.close-btn');

const url = window.location.href;
const isLogout = url.includes('/Login/Logout');

if (isLogout) {
    showNotification();
    setTimeout(() => {
        hideNotification();
    }, 15000);
}

function showNotification() {
    notification.classList.add('show');
}

function hideNotification() {
    notification.classList.remove('show');
}

closeBtn.addEventListener('click', hideNotification);