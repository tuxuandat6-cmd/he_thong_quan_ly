using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MixueDatMon.MoHinh;

// ===== Hằng số (khớp dữ liệu trong bảng) =====
public static class HangSoTrangThai
{
    public const int ChoXacNhan = 1, DangLam = 2, SanSang = 3, HoanThanh = 4, DaHuy = 5;
    public static string Ten(int ma) => ma switch
    {
        1 => "Chờ xác nhận", 2 => "Đang làm", 3 => "Sẵn sàng", 4 => "Hoàn thành", 5 => "Đã hủy", _ => "?"
    };
}

public static class HangSoPhuongThuc
{
    public const int TienMat = 1, ChuyenKhoan = 2;
    public static string Ten(int ma) => ma == TienMat ? "Tiền mặt" : ma == ChuyenKhoan ? "Chuyển khoản" : "?";
}

public static class HangSoVaiTro { public const string QuanLy = "QuanLy", ThuNgan = "ThuNgan"; }

public static class HangSoLoaiGiam
{
    public const string PhanTram = "PHAN_TRAM", SoTien = "SO_TIEN";
    public static string Ten(string ma) => ma == PhanTram ? "Phần trăm" : ma == SoTien ? "Số tiền" : ma;
}

public static class HangSoBienDong { public const string Nhap = "NHAP", XuatTheoDon = "XUAT_DON", DieuChinh = "DIEU_CHINH", HoanKho = "HOAN_KHO"; }

public static class HangSoYeuCau
{
    public const string GoiNhanVien = "GOI_NHAN_VIEN", XinThanhToan = "XIN_THANH_TOAN";
    public static string Ten(string ma) => ma == GoiNhanVien ? "Gọi nhân viên" : ma == XinThanhToan ? "Xin thanh toán" : ma;
}

/// <summary>Các loại thao tác được ghi vào nhật ký (cột HanhDong).</summary>
public static class HangSoNhatKy
{
    public const string HuyDon = "HUY_DON", HoanTien = "HOAN_TIEN", TuHuyDon = "TU_HUY_DON", ChuyenBan = "CHUYEN_BAN", TachDon = "TACH_DON",
        SuaMon = "SUA_MON", DoiTrangThaiMon = "DOI_TRANG_THAI_MON", SuaTopping = "SUA_TOPPING", SuaAnhMon = "SUA_ANH_MON",
        DieuChinhKho = "DIEU_CHINH_KHO", NhapKho = "NHAP_KHO", TaoTaiKhoan = "TAO_TAI_KHOAN", DatLaiMatKhau = "DAT_LAI_MAT_KHAU",
        KhoaMoTaiKhoan = "KHOA_MO_TAI_KHOAN", DoiMaQr = "DOI_MA_QR", SuaCaiDat = "SUA_CAI_DAT";

    public static string Ten(string ma) => ma switch
    {
        HuyDon => "Hủy đơn", HoanTien => "Hủy đơn và hoàn tiền", TuHuyDon => "Tự động hủy đơn", ChuyenBan => "Chuyển / gộp bàn",
        TachDon => "Tách bill", SuaMon => "Sửa món / giá", DoiTrangThaiMon => "Đổi trạng thái món", SuaTopping => "Sửa topping",
        SuaAnhMon => "Đổi ảnh món", DieuChinhKho => "Điều chỉnh kho", NhapKho => "Nhập kho", TaoTaiKhoan => "Tạo tài khoản",
        DatLaiMatKhau => "Đặt lại mật khẩu", KhoaMoTaiKhoan => "Khóa / mở tài khoản", DoiMaQr => "Đổi mã QR bàn", SuaCaiDat => "Sửa cài đặt quán",
        _ => ma
    };
}

// ===== Nhân sự =====
public class VaiTro
{
    [Key] public int MaVaiTro { get; set; }
    public string TenVaiTro { get; set; } = "";
}

public class NguoiDung
{
    [Key] public int MaNguoiDung { get; set; }
    public string TenDangNhap { get; set; } = "";
    public string MatKhauHash { get; set; } = "";
    public string MuoiMatKhau { get; set; } = "";
    public string HoTen { get; set; } = "";
    public string? SoDienThoai { get; set; }
    public int MaVaiTro { get; set; }
    public VaiTro VaiTro { get; set; } = null!;
    public bool DangHoatDong { get; set; } = true;
    public DateTime NgayTao { get; set; } = DateTime.Now;
    public DateTime? LanDangNhapCuoi { get; set; }
}

// ===== Thực đơn =====
public class NhomMon
{
    [Key] public int MaNhomMon { get; set; }
    public string TenNhomMon { get; set; } = "";
    public string? BieuTuong { get; set; }
    public int ThuTuHienThi { get; set; }
    public bool CoTuyChonDoUong { get; set; }
    public bool DangHoatDong { get; set; }
    public List<MonAn> DanhSachMon { get; set; } = new();
}

public class MonAn
{
    [Key] public int MaMonAn { get; set; }
    public int MaNhomMon { get; set; }
    public NhomMon NhomMon { get; set; } = null!;
    public string TenMon { get; set; } = "";
    public string? TenTiengAnh { get; set; }
    public string? MoTa { get; set; }
    public decimal Gia { get; set; }
    public string? DuongDanAnh { get; set; }
    public bool LaBanChay { get; set; }
    public bool DangBan { get; set; } = true;
    public DateTime NgayTao { get; set; } = DateTime.Now;
}

/// <summary>Danh mục size dùng chung cho mọi món (M, L, XL...).</summary>
public class KichCo
{
    [Key] public int MaKichCo { get; set; }
    public string TenKichCo { get; set; } = "";
    public int ThuTu { get; set; }
}

/// <summary>Giá riêng của 1 món theo 1 size. Món không có dòng nào ở đây = không phân size, bán theo MonAn.Gia như cũ.</summary>
public class MonAnKichCo
{
    public int MaMonAn { get; set; }
    public MonAn MonAn { get; set; } = null!;
    public int MaKichCo { get; set; }
    public KichCo KichCo { get; set; } = null!;
    public decimal Gia { get; set; }
}

public class Topping
{
    [Key] public int MaTopping { get; set; }
    public string TenTopping { get; set; } = "";
    public decimal Gia { get; set; }
    public bool DangBan { get; set; } = true;
}

public class MucDa    { [Key] public int MaMucDa { get; set; }    public string TenMuc { get; set; } = ""; }
public class MucDuong { [Key] public int MaMucDuong { get; set; } public string TenMuc { get; set; } = ""; }

public class BanAn
{
    [Key] public int MaBan { get; set; }
    public string TenBan { get; set; } = "";
    public string MaQr { get; set; } = "";
    public bool DangHoatDong { get; set; } = true;
}

// ===== Đơn hàng =====
public class TrangThaiDon { [Key] public int MaTrangThai { get; set; } public string TenTrangThai { get; set; } = ""; }
public class PhuongThucThanhToan { [Key] public int MaPhuongThuc { get; set; } public string TenPhuongThuc { get; set; } = ""; }

public class DonHang
{
    [Key] public int MaDonHang { get; set; }
    public string MaTheoDoi { get; set; } = "";
    public int? MaBan { get; set; }
    public BanAn? BanAn { get; set; }
    public int MaTrangThai { get; set; } = HangSoTrangThai.ChoXacNhan;
    public TrangThaiDon TrangThaiDon { get; set; } = null!;
    public string? TenKhach { get; set; }
    public string? GhiChuKhach { get; set; }
    public int? MaPhuongThuc { get; set; }
    public PhuongThucThanhToan? PhuongThucThanhToan { get; set; }
    public DateTime? NgayThanhToan { get; set; }
    public int? MaNguoiXuLy { get; set; }
    public NguoiDung? NguoiXuLy { get; set; }
    public int? MaNguoiThuTien { get; set; }
    public NguoiDung? NguoiThuTien { get; set; }
    public string? LyDoHuy { get; set; }
    public DateTime? NgayHuy { get; set; }
    public int? MaNguoiHuy { get; set; }
    public NguoiDung? NguoiHuy { get; set; }
    public DateTime? NgayHoanTien { get; set; }
    public DateTime NgayTao { get; set; } = DateTime.Now;
    public DateTime? NgayCapNhat { get; set; }
    public List<ChiTietDonHang> DanhSachChiTiet { get; set; } = new();
    public DanhGiaDonHang? DanhGia { get; set; }

    public string? SoDienThoaiKhach { get; set; }
    public int? MaGiamGiaApDung { get; set; }
    public MaGiamGia? MaGiamGia { get; set; }
    public decimal SoTienGiam { get; set; }
    public int DiemDaTich { get; set; }

    /// <summary>Tổng tiền món + topping, chưa trừ giảm giá.</summary>
    [NotMapped] public decimal TongTienGoc => DanhSachChiTiet.Sum(c => c.ThanhTien);
    /// <summary>Số tiền khách thực trả = tiền món trừ giảm giá, không âm. Khớp cột TongTien của view vw_TongTienDonHang.</summary>
    [NotMapped] public decimal TongTien => Math.Max(0, TongTienGoc - SoTienGiam);
    [NotMapped] public string NoiPhucVu => BanAn?.TenBan ?? "Mang đi";
}

public class ChiTietDonHang
{
    [Key] public int MaChiTiet { get; set; }
    public int MaDonHang { get; set; }
    public DonHang DonHang { get; set; } = null!;
    public int MaMonAn { get; set; }
    public MonAn MonAn { get; set; } = null!;
    public int SoLuong { get; set; }
    public decimal DonGia { get; set; }
    public int? MaMucDa { get; set; }
    public MucDa? MucDa { get; set; }
    public int? MaMucDuong { get; set; }
    public MucDuong? MucDuong { get; set; }
    public int? MaKichCo { get; set; }
    public KichCo? KichCo { get; set; }
    public string? GhiChu { get; set; }
    public List<ChiTietTopping> DanhSachTopping { get; set; } = new();
    public DanhGiaMon? DanhGia { get; set; }

    [NotMapped] public decimal ThanhTien => (DonGia + DanhSachTopping.Sum(t => t.DonGia)) * SoLuong;
}

public class ChiTietTopping
{
    public int MaChiTiet { get; set; }
    public ChiTietDonHang ChiTietDonHang { get; set; } = null!;
    public int MaTopping { get; set; }
    public Topping Topping { get; set; } = null!;
    public decimal DonGia { get; set; }
}

/// <summary>Ánh xạ view vw_TongTienDonHang (không khóa).</summary>
public class TongTienDon { public int MaDonHang { get; set; } public decimal TongTien { get; set; } public decimal TongTienGoc { get; set; } }

// ===== Kho =====
public class NguyenLieu
{
    [Key] public int MaNguyenLieu { get; set; }
    public string TenNguyenLieu { get; set; } = "";
    public string DonViTinh { get; set; } = "";
    [Column(TypeName = "decimal(18,3)")] public decimal SoLuongTon { get; set; }
    [Column(TypeName = "decimal(18,3)")] public decimal SoLuongToiThieu { get; set; }
    public bool DangHoatDong { get; set; } = true;
}

/// <summary>Định mức nguyên liệu cho 1 phần món. MaKichCo = NULL: công thức mặc định (món không phân size).
/// Món có size thì mỗi size nên có bộ công thức riêng (MaKichCo khớp với dòng trong MonAnKichCo).</summary>
public class CongThucMon
{
    [Key] public int MaCongThucMon { get; set; }
    public int MaMonAn { get; set; }
    public int? MaKichCo { get; set; }
    public KichCo? KichCo { get; set; }
    public int MaNguyenLieu { get; set; }
    [Column(TypeName = "decimal(18,3)")] public decimal SoLuongDung { get; set; }
}

public class CongThucTopping
{
    public int MaTopping { get; set; }
    public int MaNguyenLieu { get; set; }
    [Column(TypeName = "decimal(18,3)")] public decimal SoLuongDung { get; set; }
}

public class NhaCungCap
{
    [Key] public int MaNhaCungCap { get; set; }
    public string TenNhaCungCap { get; set; } = "";
    public string? SoDienThoai { get; set; }
    public string? DiaChi { get; set; }
}

public class PhieuNhap
{
    [Key] public int MaPhieuNhap { get; set; }
    public int? MaNhaCungCap { get; set; }
    public NhaCungCap? NhaCungCap { get; set; }
    public int MaNguoiNhap { get; set; }
    public DateTime NgayNhap { get; set; } = DateTime.Now;
    public string? GhiChu { get; set; }
    public List<ChiTietPhieuNhap> DanhSachChiTiet { get; set; } = new();
}

public class ChiTietPhieuNhap
{
    [Key] public int MaChiTietNhap { get; set; }
    public int MaPhieuNhap { get; set; }
    public int MaNguyenLieu { get; set; }
    [Column(TypeName = "decimal(18,3)")] public decimal SoLuong { get; set; }
    public decimal DonGiaNhap { get; set; }
}

public class BienDongKho
{
    [Key] public long MaBienDong { get; set; }
    public int MaNguyenLieu { get; set; }
    public NguyenLieu NguyenLieu { get; set; } = null!;
    [Column(TypeName = "decimal(18,3)")] public decimal SoLuongThayDoi { get; set; }
    public string LoaiBienDong { get; set; } = "";
    public int? MaDonHang { get; set; }
    public int? MaPhieuNhap { get; set; }
    public int? MaNguoiThucHien { get; set; }
    public DateTime NgayTao { get; set; } = DateTime.Now;
    public string? GhiChu { get; set; }
}

// ===== Đánh giá =====
public class DanhGiaMon
{
    [Key] public int MaDanhGiaMon { get; set; }
    public int MaChiTiet { get; set; }
    public ChiTietDonHang ChiTietDonHang { get; set; } = null!;
    public int SoSao { get; set; }
    public string? NhanXet { get; set; }
    public DateTime NgayTao { get; set; } = DateTime.Now;
    public bool DaAn { get; set; }
}

public class DanhGiaDonHang
{
    [Key] public int MaDanhGiaDon { get; set; }
    public int MaDonHang { get; set; }
    public DonHang DonHang { get; set; } = null!;
    public int SoSao { get; set; }
    public string? GopY { get; set; }
    public DateTime NgayTao { get; set; } = DateTime.Now;
    public bool DaXem { get; set; }
}

/// <summary>Ánh xạ view vw_ThongKeDanhGiaMon (không khóa): số lượt và điểm trung bình mỗi món.</summary>
public class ThongKeDanhGia
{
    public int MaMonAn { get; set; }
    public int SoLuot { get; set; }
    [Column(TypeName = "decimal(3,2)")] public decimal DiemTrungBinh { get; set; }
}

// ===== Đối soát tiền cuối ngày =====
public class DoiSoatCuoiNgay
{
    [Key] public int MaDoiSoat { get; set; }
    [Column(TypeName = "date")] public DateTime NgayDoiSoat { get; set; }
    public int MaNguoiThucHien { get; set; }
    public NguoiDung NguoiThucHien { get; set; } = null!;
    public decimal TienDauCa { get; set; }
    public decimal TienMatHeThong { get; set; }
    public decimal TienMatThucDem { get; set; }
    public string? GhiChu { get; set; }
    public DateTime NgayTao { get; set; } = DateTime.Now;

    /// <summary>Dương = thừa tiền, âm = thiếu tiền, 0 = khớp.</summary>
    [NotMapped] public decimal ChenhLech => TienMatThucDem - (TienDauCa + TienMatHeThong);
}

// ===== Vận hành: cài đặt quán, nhật ký, yêu cầu từ bàn =====
public class CaiDatCuaHang
{
    [Key] public int MaCaiDat { get; set; } = 1;
    public TimeSpan GioMo { get; set; } = new(8, 0, 0);
    public TimeSpan GioDong { get; set; } = new(22, 0, 0);
    public bool TamNgungNhanDon { get; set; }
    public int SoPhutTuHuyDonMangDi { get; set; } = 15;
    public bool TuAnMonHetNguyenLieu { get; set; } = true;
    public int SoDongTichMotDiem { get; set; } = 10000;
}

public class NhatKyThaoTac
{
    [Key] public long MaNhatKy { get; set; }
    public DateTime NgayTao { get; set; } = DateTime.Now;
    public int? MaNguoiThucHien { get; set; }
    public NguoiDung? NguoiThucHien { get; set; }
    public string HanhDong { get; set; } = "";
    public string MoTa { get; set; } = "";
    public int? MaDonHang { get; set; }
}

public class YeuCauTaiBan
{
    [Key] public int MaYeuCau { get; set; }
    public int MaBan { get; set; }
    public BanAn BanAn { get; set; } = null!;
    public string LoaiYeuCau { get; set; } = "";
    public DateTime NgayTao { get; set; } = DateTime.Now;
    public bool DaXuLy { get; set; }
    public DateTime? NgayXuLy { get; set; }
    public int? MaNguoiXuLy { get; set; }
    public NguoiDung? NguoiXuLy { get; set; }
}

/// <summary>Khách hàng thân thiết, định danh theo số điện thoại (khớp bảng KhachHang).</summary>
public class KhachHang
{
    [Key] public string SoDienThoai { get; set; } = "";
    public string? HoTen { get; set; }
    public int DiemTichLuy { get; set; }
    public decimal TongChiTieu { get; set; }
    public DateTime NgayTao { get; set; } = DateTime.Now;
    public DateTime? LanCuoiMua { get; set; }
}

/// <summary>Mã giảm giá (khớp bảng MaGiamGia).</summary>
public class MaGiamGia
{
    [Key] public int MaMaGiamGia { get; set; }
    public string MaCode { get; set; } = "";
    public string? MoTa { get; set; }
    public string LoaiGiam { get; set; } = HangSoLoaiGiam.SoTien;
    public decimal GiaTriGiam { get; set; }
    public decimal? GiamToiDa { get; set; }
    public decimal DonHangToiThieu { get; set; }
    public int? SoLuotToiDa { get; set; }
    public int SoLuotDaDung { get; set; }
    public DateTime? NgayBatDau { get; set; }
    public DateTime? NgayKetThuc { get; set; }
    public bool DangHoatDong { get; set; } = true;
    public DateTime NgayTao { get; set; } = DateTime.Now;
}
