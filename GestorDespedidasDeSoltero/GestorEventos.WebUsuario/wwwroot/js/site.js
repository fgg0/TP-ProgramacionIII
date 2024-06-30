// Código para la animación del header cuando hay scroll
window.addEventListener('scroll', function () {
    var header = document.querySelector('header');
    if (window.scrollY > 50) {
        header.classList.add('scrolled');
    } else {
        header.classList.remove('scrolled');
    }
});

// Código del carrousel de eventos
const carousel = document.querySelector('.carousel-inner');
const items = carousel.querySelectorAll('.carousel-item');
const prevBtn = document.querySelector('.carousel-control.prev');
const nextBtn = document.querySelector('.carousel-control.next');
let currentIndex = 0;

function showItem(index) {
    carousel.style.transform = `translateX(-${index * 100}%)`;
}

prevBtn.addEventListener('click', () => {
    currentIndex = (currentIndex - 1 + items.length) % items.length;
    showItem(currentIndex);
});

nextBtn.addEventListener('click', () => {
    currentIndex = (currentIndex + 1) % items.length;
    showItem(currentIndex);
});

// Modal
function showModal(modalId) {
    document.getElementById(modalId).style.display = 'block';
}

function closeModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.style.animation = 'fadeOut 0.3s ease-out forwards, slideOut 0.3s ease-out forwards';
        setTimeout(() => {
            modal.style.display = 'none';
            modal.style.animation = '';
        }, 300);
    }
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
                if (window.getComputedStyle(modal).display === 'block') {
                    closeModal(modal.id);
                }
            });
        }
    });
}


// Animación del feedback
const fbtrack = document.querySelector('.feedback-track');
const fbitems = track.querySelectorAll('.feedback-item');

// Clonar los items para crear un bucle infinito
items.forEach(fbitems => {
    const clone = fbitems.cloneNode(true);
    track.appendChild(clone);
});

let position = 0;
const totalWidth = items.length * (fbitems[0].offsetWidth + 20); // 20 es el margen total

function moveTrack() {
    position -= 1; // Mover 1 píxel cada frame
    if (Math.abs(position) >= totalWidth / 2) {
        position = 0;
    }
    track.style.transform = `translateX(${position}px)`;
    requestAnimationFrame(moveTrack);
}

moveTrack();

document.addEventListener('DOMContentLoaded', function () {
    setupModalEvents();
});