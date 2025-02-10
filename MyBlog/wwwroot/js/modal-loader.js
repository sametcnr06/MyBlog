document.addEventListener("DOMContentLoaded", function () {
    const modal = document.getElementById("rolAtamaModal");
    const modalBody = modal.querySelector("#rolAtamaModalBody");

    // Modal açıldığında içerik yükleme
    modal.addEventListener("show.bs.modal", function (event) {
        const button = event.relatedTarget; // Modalı açan buton
        const userId = button.getAttribute("data-user-id"); // Kullanıcı ID'sini al

        // Modal içeriğini temizle
        modalBody.innerHTML = "<p>Yükleniyor...</p>";

        // Dinamik içerik yükleme
        fetch(`/Admin/RolAtamaPartial?id=${userId}`)
            .then((response) => response.text())
            .then((html) => {
                modalBody.innerHTML = html; // Gelen HTML içeriğini modalda göster
            })
            .catch((error) => {
                console.error("Hata:", error);
                modalBody.innerHTML = "<p>Bir hata oluştu. Lütfen tekrar deneyin.</p>";
            });
    });

    // Form gönderimi sırasında işlemler
    document.addEventListener("submit", function (event) {
        if (event.target && event.target.id === "rolAtamaForm") {
            event.preventDefault(); // Varsayılan form gönderimini engelle

            const formData = new FormData(event.target);
            fetch(event.target.action, {
                method: "POST",
                body: formData,
            })
                .then((response) => {
                    if (response.ok) {
                        // Modalı kapat ve sayfayı yenile
                        const modalInstance = bootstrap.Modal.getInstance(modal);
                        modalInstance.hide();
                        location.reload(); // Sayfayı yenile
                    } else {
                        throw new Error("Form gönderilirken hata oluştu.");
                    }
                })
                .catch((error) => console.error("Hata:", error));
        }
    });
});
