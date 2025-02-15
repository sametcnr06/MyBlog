@model List < MyBlog.Entities.Post >

    @{
        Layout = "~/Views/Home/_HomeLayout.cshtml";
        ViewData["Title"] = "Tüm Blog Yazýlarý";
    }

    < !--Özel stiller: -->
<style>
    /* Kart geçiþ animasyonu ve gölge efekti */
    .custom-card {
        border: none;
        overflow: hidden;
        transition: transform 0.3s ease, box-shadow 0.3s ease;
        border-radius: 0.5rem;
    }
    .custom-card:hover {
        transform: translateY(-5px);
        box-shadow: 0 8px 20px rgba(0, 0, 0, 0.15);
    }
    /* Kart üzerindeki resim */
    .custom-card img {
        width: 100%;
        height: 200px;
        object-fit: cover;
        display: block;
    }
    /* Overlay efekti için konteyner */
    .card-overlay {
        position: relative;
    }
    .card-overlay::after {
        content: "";
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: linear-gradient(to bottom, transparent, rgba(0, 0, 0, 0.6));
        opacity: 0;
        transition: opacity 0.3s ease;
    }
    .card-overlay:hover::after {
        opacity: 1;
    }
    /* Overlay üzerindeki yazýlar */
    .overlay-text {
        position: absolute;
        bottom: 0;
        left: 0;
        right: 0;
        padding: 1rem;
        color: #fff;
        z-index: 2;
    }
    .overlay-text h5 {
        margin: 0;
        font-size: 1.2rem;
        font-weight: bold;
    }
    .overlay-text p {
        margin: 0;
        font-size: 0.9rem;
    }
</style>

<div class="container mt-4">
    <h2 class="text-center mb-4">Tüm Blog Yazýlarý</h2>

    @if (Model == null || !Model.Any())
    {
        <p class="text-center">Henüz hiç yazý eklenmemiþ.</p>
    }
    else
    {
        <div class="row">
            @foreach (var post in Model)
            {
                <div class="col-md-6 col-lg-4 mb-4">
                    <div class="card custom-card">
                        <div class="card-overlay">
                            <img src="@post.ImageUrl" alt="@post.Title" loading="lazy" />
                            <div class="overlay-text">
                                <h5>@post.Title</h5>
                                <p>Yazar: @post.Author.UserName</p>
                            </div>
                        </div>
                        <div class="card-body">
                            <p class="card-text">
                                <small class="text-muted">Kategori: @post.Category.CategoryName</small>
                            </p>
                            <a href="/Home/PostDetail/@post.Id" class="btn btn-primary btn-block">Detaylarý Gör</a>
                        </div>
                    </div>
                </div>
            }
        </div>
    }
</div>
