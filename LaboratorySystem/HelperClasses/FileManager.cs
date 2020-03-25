using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace LaboratorySystem
{
    public class FileManager
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public Byte[] FileBytes { get; set; }
        public string MimeType { get; set; }



        private List<KeyValuePair<string, string>> _mimetypes = new List<KeyValuePair<string, string>>();

        private void MimeTypeInitialization()
        {
            #region MineTypes
            _mimetypes.Add(new KeyValuePair<string, string>(".x3d", "application/vnd.hzn-3d-crossword"));
            _mimetypes.Add(new KeyValuePair<string, string>(".3gp", "video/3gpp"));
            _mimetypes.Add(new KeyValuePair<string, string>(".3g2", "video/3gpp2"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mseq", "application/vnd.mseq"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pwn", "application/vnd.3m.post-it-notes"));
            _mimetypes.Add(new KeyValuePair<string, string>(".plb", "application/vnd.3gpp.pic-bw-large"));
            _mimetypes.Add(new KeyValuePair<string, string>(".psb", "application/vnd.3gpp.pic-bw-small"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pvb", "application/vnd.3gpp.pic-bw-var"));
            _mimetypes.Add(new KeyValuePair<string, string>(".tcap", "application/vnd.3gpp2.tcap"));
            _mimetypes.Add(new KeyValuePair<string, string>(".7z", "application/x-7z-compressed"));
            _mimetypes.Add(new KeyValuePair<string, string>(".abw", "application/x-abiword"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ace", "application/x-ace-compressed"));
            _mimetypes.Add(new KeyValuePair<string, string>(".acc", "application/vnd.americandynamics.acc"));
            _mimetypes.Add(new KeyValuePair<string, string>(".acu", "application/vnd.acucobol"));
            _mimetypes.Add(new KeyValuePair<string, string>(".atc", "application/vnd.acucorp"));
            _mimetypes.Add(new KeyValuePair<string, string>(".adp", "audio/adpcm"));
            _mimetypes.Add(new KeyValuePair<string, string>(".aab", "application/x-authorware-bin"));
            _mimetypes.Add(new KeyValuePair<string, string>(".aam", "application/x-authorware-map"));
            _mimetypes.Add(new KeyValuePair<string, string>(".aas", "application/x-authorware-seg"));
            _mimetypes.Add(new KeyValuePair<string, string>(".air", "application/vnd.adobe.air-application-installer-package+zip"));
            _mimetypes.Add(new KeyValuePair<string, string>(".swf", "application/x-shockwave-flash"));
            _mimetypes.Add(new KeyValuePair<string, string>(".fxp", "application/vnd.adobe.fxp"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pdf", "application/pdf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ppd", "application/vnd.cups-ppd"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dir", "application/x-director"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xdp", "application/vnd.adobe.xdp+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xfdf", "application/vnd.adobe.xfdf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".aac", "audio/x-aac"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ahead", "application/vnd.ahead.space"));
            _mimetypes.Add(new KeyValuePair<string, string>(".azf", "application/vnd.airzip.filesecure.azf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".azs", "application/vnd.airzip.filesecure.azs"));
            _mimetypes.Add(new KeyValuePair<string, string>(".azw", "application/vnd.amazon.ebook"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ami", "application/vnd.amiga.ami"));
            _mimetypes.Add(new KeyValuePair<string, string>("N/A", "application/andrew-inset"));
            _mimetypes.Add(new KeyValuePair<string, string>(".apk", "application/vnd.android.package-archive"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cii", "application/vnd.anser-web-certificate-issue-initiation"));
            _mimetypes.Add(new KeyValuePair<string, string>(".fti", "application/vnd.anser-web-funds-transfer-initiation"));
            _mimetypes.Add(new KeyValuePair<string, string>(".atx", "application/vnd.antix.game-component"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dmg", "application/x-apple-diskimage"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mpkg", "application/vnd.apple.installer+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".aw", "application/applixware"));
            _mimetypes.Add(new KeyValuePair<string, string>(".les", "application/vnd.hhe.lesson-player"));
            _mimetypes.Add(new KeyValuePair<string, string>(".swi", "application/vnd.aristanetworks.swi"));
            _mimetypes.Add(new KeyValuePair<string, string>(".s", "text/x-asm"));
            _mimetypes.Add(new KeyValuePair<string, string>(".atomcat", "application/atomcat+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".atomsvc", "application/atomsvc+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".atom, .xml", "application/atom+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ac", "application/pkix-attr-cert"));
            _mimetypes.Add(new KeyValuePair<string, string>(".aif", "audio/x-aiff"));
            _mimetypes.Add(new KeyValuePair<string, string>(".avi", "video/x-msvideo"));
            _mimetypes.Add(new KeyValuePair<string, string>(".aep", "application/vnd.audiograph"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dxf", "image/vnd.dxf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dwf", "model/vnd.dwf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".par", "text/plain-bas"));
            _mimetypes.Add(new KeyValuePair<string, string>(".bcpio", "application/x-bcpio"));
            _mimetypes.Add(new KeyValuePair<string, string>(".bin", "application/octet-stream"));
            _mimetypes.Add(new KeyValuePair<string, string>(".bmp", "image/bmp"));
            _mimetypes.Add(new KeyValuePair<string, string>(".torrent", "application/x-bittorrent"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cod", "application/vnd.rim.cod"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mpm", "application/vnd.blueice.multipass"));
            _mimetypes.Add(new KeyValuePair<string, string>(".bmi", "application/vnd.bmi"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sh", "application/x-sh"));
            _mimetypes.Add(new KeyValuePair<string, string>(".btif", "image/prs.btif"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rep", "application/vnd.businessobjects"));
            _mimetypes.Add(new KeyValuePair<string, string>(".bz", "application/x-bzip"));
            _mimetypes.Add(new KeyValuePair<string, string>(".bz2", "application/x-bzip2"));
            _mimetypes.Add(new KeyValuePair<string, string>(".csh", "application/x-csh"));
            _mimetypes.Add(new KeyValuePair<string, string>(".c", "text/x-c"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cdxml", "application/vnd.chemdraw+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".css", "text/css"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cdx", "chemical/x-cdx"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cml", "chemical/x-cml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".csml", "chemical/x-csml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cdbcmsg", "application/vnd.contact.cmsg"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cla", "application/vnd.claymore"));
            _mimetypes.Add(new KeyValuePair<string, string>(".c4g", "application/vnd.clonk.c4group"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sub", "image/vnd.dvb.subtitle"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cdmia", "application/cdmi-capability"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cdmic", "application/cdmi-container"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cdmid", "application/cdmi-domain"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cdmio", "application/cdmi-object"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cdmiq", "application/cdmi-queue"));
            _mimetypes.Add(new KeyValuePair<string, string>(".c11amc", "application/vnd.cluetrust.cartomobile-config"));
            _mimetypes.Add(new KeyValuePair<string, string>(".c11amz", "application/vnd.cluetrust.cartomobile-config-pkg"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ras", "image/x-cmu-raster"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dae", "model/vnd.collada+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".csv", "text/csv"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cpt", "application/mac-compactpro"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wmlc", "application/vnd.wap.wmlc"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cgm", "image/cgm"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ice", "x-conference/x-cooltalk"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cmx", "image/x-cmx"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xar", "application/vnd.xara"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cmc", "application/vnd.cosmocaller"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cpio", "application/x-cpio"));
            _mimetypes.Add(new KeyValuePair<string, string>(".clkx", "application/vnd.crick.clicker"));
            _mimetypes.Add(new KeyValuePair<string, string>(".clkk", "application/vnd.crick.clicker.keyboard"));
            _mimetypes.Add(new KeyValuePair<string, string>(".clkp", "application/vnd.crick.clicker.palette"));
            _mimetypes.Add(new KeyValuePair<string, string>(".clkt", "application/vnd.crick.clicker.template"));
            _mimetypes.Add(new KeyValuePair<string, string>(".clkw", "application/vnd.crick.clicker.wordbank"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wbs", "application/vnd.criticaltools.wbs+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cryptonote", "application/vnd.rig.cryptonote"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cif", "chemical/x-cif"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cmdf", "chemical/x-cmdf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cu", "application/cu-seeme"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cww", "application/prs.cww"));
            _mimetypes.Add(new KeyValuePair<string, string>(".curl", "text/vnd.curl"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dcurl", "text/vnd.curl.dcurl"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mcurl", "text/vnd.curl.mcurl"));
            _mimetypes.Add(new KeyValuePair<string, string>(".scurl", "text/vnd.curl.scurl"));
            _mimetypes.Add(new KeyValuePair<string, string>(".car", "application/vnd.curl.car"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pcurl", "application/vnd.curl.pcurl"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cmp", "application/vnd.yellowriver-custom-menu"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dssc", "application/dssc+der"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xdssc", "application/dssc+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".deb", "application/x-debian-package"));
            _mimetypes.Add(new KeyValuePair<string, string>(".uva", "audio/vnd.dece.audio"));
            _mimetypes.Add(new KeyValuePair<string, string>(".uvi", "image/vnd.dece.graphic"));
            _mimetypes.Add(new KeyValuePair<string, string>(".uvh", "video/vnd.dece.hd"));
            _mimetypes.Add(new KeyValuePair<string, string>(".uvm", "video/vnd.dece.mobile"));
            _mimetypes.Add(new KeyValuePair<string, string>(".uvu", "video/vnd.uvvu.mp4"));
            _mimetypes.Add(new KeyValuePair<string, string>(".uvp", "video/vnd.dece.pd"));
            _mimetypes.Add(new KeyValuePair<string, string>(".uvs", "video/vnd.dece.sd"));
            _mimetypes.Add(new KeyValuePair<string, string>(".uvv", "video/vnd.dece.video"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dvi", "application/x-dvi"));
            _mimetypes.Add(new KeyValuePair<string, string>(".seed", "application/vnd.fdsn.seed"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dtb", "application/x-dtbook+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".res", "application/x-dtbresource+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ait", "application/vnd.dvb.ait"));
            _mimetypes.Add(new KeyValuePair<string, string>(".svc", "application/vnd.dvb.service"));
            _mimetypes.Add(new KeyValuePair<string, string>(".eol", "audio/vnd.digital-winds"));
            _mimetypes.Add(new KeyValuePair<string, string>(".djvu", "image/vnd.djvu"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dtd", "application/xml-dtd"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mlp", "application/vnd.dolby.mlp"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wad", "application/x-doom"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dpg", "application/vnd.dpgraph"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dra", "audio/vnd.dra"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dfac", "application/vnd.dreamfactory"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dts", "audio/vnd.dts"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dtshd", "audio/vnd.dts.hd"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dwg", "image/vnd.dwg"));
            _mimetypes.Add(new KeyValuePair<string, string>(".geo", "application/vnd.dynageo"));
            _mimetypes.Add(new KeyValuePair<string, string>(".es", "application/ecmascript"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mag", "application/vnd.ecowin.chart"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mmr", "image/vnd.fujixerox.edmics-mmr"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rlc", "image/vnd.fujixerox.edmics-rlc"));
            _mimetypes.Add(new KeyValuePair<string, string>(".exi", "application/exi"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mgz", "application/vnd.proteus.magazine"));
            _mimetypes.Add(new KeyValuePair<string, string>(".epub", "application/epub+zip"));
            _mimetypes.Add(new KeyValuePair<string, string>(".eml", "message/rfc822"));
            _mimetypes.Add(new KeyValuePair<string, string>(".nml", "application/vnd.enliven"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xpr", "application/vnd.is-xpr"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xif", "image/vnd.xiff"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xfdl", "application/vnd.xfdl"));
            _mimetypes.Add(new KeyValuePair<string, string>(".emma", "application/emma+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ez2", "application/vnd.ezpix-album"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ez3", "application/vnd.ezpix-package"));
            _mimetypes.Add(new KeyValuePair<string, string>(".fst", "image/vnd.fst"));
            _mimetypes.Add(new KeyValuePair<string, string>(".fvt", "video/vnd.fvt"));
            _mimetypes.Add(new KeyValuePair<string, string>(".fbs", "image/vnd.fastbidsheet"));
            _mimetypes.Add(new KeyValuePair<string, string>(".fe_launch", "application/vnd.denovo.fcselayout-link"));
            _mimetypes.Add(new KeyValuePair<string, string>(".f4v", "video/x-f4v"));
            _mimetypes.Add(new KeyValuePair<string, string>(".flv", "video/x-flv"));
            _mimetypes.Add(new KeyValuePair<string, string>(".fpx", "image/vnd.fpx"));
            _mimetypes.Add(new KeyValuePair<string, string>(".npx", "image/vnd.net-fpx"));
            _mimetypes.Add(new KeyValuePair<string, string>(".flx", "text/vnd.fmi.flexstor"));
            _mimetypes.Add(new KeyValuePair<string, string>(".fli", "video/x-fli"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ftc", "application/vnd.fluxtime.clip"));
            _mimetypes.Add(new KeyValuePair<string, string>(".fdf", "application/vnd.fdf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".f", "text/x-fortran"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mif", "application/vnd.mif"));
            _mimetypes.Add(new KeyValuePair<string, string>(".fm", "application/vnd.framemaker"));
            _mimetypes.Add(new KeyValuePair<string, string>(".fh", "image/x-freehand"));
            _mimetypes.Add(new KeyValuePair<string, string>(".fsc", "application/vnd.fsc.weblaunch"));
            _mimetypes.Add(new KeyValuePair<string, string>(".fnc", "application/vnd.frogans.fnc"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ltf", "application/vnd.frogans.ltf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ddd", "application/vnd.fujixerox.ddd"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xdw", "application/vnd.fujixerox.docuworks"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xbd", "application/vnd.fujixerox.docuworks.binder"));
            _mimetypes.Add(new KeyValuePair<string, string>(".oas", "application/vnd.fujitsu.oasys"));
            _mimetypes.Add(new KeyValuePair<string, string>(".oa2", "application/vnd.fujitsu.oasys2"));
            _mimetypes.Add(new KeyValuePair<string, string>(".oa3", "application/vnd.fujitsu.oasys3"));
            _mimetypes.Add(new KeyValuePair<string, string>(".fg5", "application/vnd.fujitsu.oasysgp"));
            _mimetypes.Add(new KeyValuePair<string, string>(".bh2", "application/vnd.fujitsu.oasysprs"));
            _mimetypes.Add(new KeyValuePair<string, string>(".spl", "application/x-futuresplash"));
            _mimetypes.Add(new KeyValuePair<string, string>(".fzs", "application/vnd.fuzzysheet"));
            _mimetypes.Add(new KeyValuePair<string, string>(".g3", "image/g3fax"));
            _mimetypes.Add(new KeyValuePair<string, string>(".gmx", "application/vnd.gmx"));
            _mimetypes.Add(new KeyValuePair<string, string>(".gtw", "model/vnd.gtw"));
            _mimetypes.Add(new KeyValuePair<string, string>(".txd", "application/vnd.genomatix.tuxedo"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ggb", "application/vnd.geogebra.file"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ggt", "application/vnd.geogebra.tool"));
            _mimetypes.Add(new KeyValuePair<string, string>(".gdl", "model/vnd.gdl"));
            _mimetypes.Add(new KeyValuePair<string, string>(".gex", "application/vnd.geometry-explorer"));
            _mimetypes.Add(new KeyValuePair<string, string>(".gxt", "application/vnd.geonext"));
            _mimetypes.Add(new KeyValuePair<string, string>(".g2w", "application/vnd.geoplan"));
            _mimetypes.Add(new KeyValuePair<string, string>(".g3w", "application/vnd.geospace"));
            _mimetypes.Add(new KeyValuePair<string, string>(".gsf", "application/x-font-ghostscript"));
            _mimetypes.Add(new KeyValuePair<string, string>(".bdf", "application/x-font-bdf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".gtar", "application/x-gtar"));
            _mimetypes.Add(new KeyValuePair<string, string>(".texinfo", "application/x-texinfo"));
            _mimetypes.Add(new KeyValuePair<string, string>(".gnumeric", "application/x-gnumeric"));
            _mimetypes.Add(new KeyValuePair<string, string>(".kml", "application/vnd.google-earth.kml+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".kmz", "application/vnd.google-earth.kmz"));
            _mimetypes.Add(new KeyValuePair<string, string>(".gqf", "application/vnd.grafeq"));
            _mimetypes.Add(new KeyValuePair<string, string>(".gif", "image/gif"));
            _mimetypes.Add(new KeyValuePair<string, string>(".gv", "text/vnd.graphviz"));
            _mimetypes.Add(new KeyValuePair<string, string>(".gac", "application/vnd.groove-account"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ghf", "application/vnd.groove-help"));
            _mimetypes.Add(new KeyValuePair<string, string>(".gim", "application/vnd.groove-identity-message"));
            _mimetypes.Add(new KeyValuePair<string, string>(".grv", "application/vnd.groove-injector"));
            _mimetypes.Add(new KeyValuePair<string, string>(".gtm", "application/vnd.groove-tool-message"));
            _mimetypes.Add(new KeyValuePair<string, string>(".tpl", "application/vnd.groove-tool-template"));
            _mimetypes.Add(new KeyValuePair<string, string>(".vcg", "application/vnd.groove-vcard"));
            _mimetypes.Add(new KeyValuePair<string, string>(".h261", "video/h261"));
            _mimetypes.Add(new KeyValuePair<string, string>(".h263", "video/h263"));
            _mimetypes.Add(new KeyValuePair<string, string>(".h264", "video/h264"));
            _mimetypes.Add(new KeyValuePair<string, string>(".hpid", "application/vnd.hp-hpid"));
            _mimetypes.Add(new KeyValuePair<string, string>(".hps", "application/vnd.hp-hps"));
            _mimetypes.Add(new KeyValuePair<string, string>(".hdf", "application/x-hdf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rip", "audio/vnd.rip"));
            _mimetypes.Add(new KeyValuePair<string, string>(".hbci", "application/vnd.hbci"));
            _mimetypes.Add(new KeyValuePair<string, string>(".jlt", "application/vnd.hp-jlyt"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pcl", "application/vnd.hp-pcl"));
            _mimetypes.Add(new KeyValuePair<string, string>(".hpgl", "application/vnd.hp-hpgl"));
            _mimetypes.Add(new KeyValuePair<string, string>(".hvs", "application/vnd.yamaha.hv-script"));
            _mimetypes.Add(new KeyValuePair<string, string>(".hvd", "application/vnd.yamaha.hv-dic"));
            _mimetypes.Add(new KeyValuePair<string, string>(".hvp", "application/vnd.yamaha.hv-voice"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sfd-hdstx", "application/vnd.hydrostatix.sof-data"));
            _mimetypes.Add(new KeyValuePair<string, string>(".stk", "application/hyperstudio"));
            _mimetypes.Add(new KeyValuePair<string, string>(".hal", "application/vnd.hal+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".html", "text/html"));
            _mimetypes.Add(new KeyValuePair<string, string>(".irm", "application/vnd.ibm.rights-management"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sc", "application/vnd.ibm.secure-container"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ics", "text/calendar"));
            _mimetypes.Add(new KeyValuePair<string, string>(".icc", "application/vnd.iccprofile"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ico", "image/x-icon"));
            _mimetypes.Add(new KeyValuePair<string, string>(".igl", "application/vnd.igloader"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ief", "image/ief"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ivp", "application/vnd.immervision-ivp"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ivu", "application/vnd.immervision-ivu"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rif", "application/reginfo+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".3dml", "text/vnd.in3d.3dml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".spot", "text/vnd.in3d.spot"));
            _mimetypes.Add(new KeyValuePair<string, string>(".igs", "model/iges"));
            _mimetypes.Add(new KeyValuePair<string, string>(".i2g", "application/vnd.intergeo"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cdy", "application/vnd.cinderella"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xpw", "application/vnd.intercon.formnet"));
            _mimetypes.Add(new KeyValuePair<string, string>(".fcs", "application/vnd.isac.fcs"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ipfix", "application/ipfix"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cer", "application/pkix-cert"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pki", "application/pkixcmp"));
            _mimetypes.Add(new KeyValuePair<string, string>(".crl", "application/pkix-crl"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pkipath", "application/pkix-pkipath"));
            _mimetypes.Add(new KeyValuePair<string, string>(".igm", "application/vnd.insors.igm"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rcprofile", "application/vnd.ipunplugged.rcprofile"));
            _mimetypes.Add(new KeyValuePair<string, string>(".irp", "application/vnd.irepository.package+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".jad", "text/vnd.sun.j2me.app-descriptor"));
            _mimetypes.Add(new KeyValuePair<string, string>(".jar", "application/java-archive"));
            _mimetypes.Add(new KeyValuePair<string, string>(".class", "application/java-vm"));
            _mimetypes.Add(new KeyValuePair<string, string>(".jnlp", "application/x-java-jnlp-file"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ser", "application/java-serialized-object"));
            _mimetypes.Add(new KeyValuePair<string, string>(".java", "text/x-java-source,java"));
            _mimetypes.Add(new KeyValuePair<string, string>(".js", "application/javascript"));
            _mimetypes.Add(new KeyValuePair<string, string>(".json", "application/json"));
            _mimetypes.Add(new KeyValuePair<string, string>(".joda", "application/vnd.joost.joda-archive"));
            _mimetypes.Add(new KeyValuePair<string, string>(".jpm", "video/jpm"));
            _mimetypes.Add(new KeyValuePair<string, string>(".jpeg, .jpg", "image/jpeg"));
            _mimetypes.Add(new KeyValuePair<string, string>(".jpeg, .jpg", "image/x-citrix-jpeg"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pjpeg", "image/pjpeg"));
            _mimetypes.Add(new KeyValuePair<string, string>(".jpgv", "video/jpeg"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ktz", "application/vnd.kahootz"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mmd", "application/vnd.chipnuts.karaoke-mmd"));
            _mimetypes.Add(new KeyValuePair<string, string>(".karbon", "application/vnd.kde.karbon"));
            _mimetypes.Add(new KeyValuePair<string, string>(".chrt", "application/vnd.kde.kchart"));
            _mimetypes.Add(new KeyValuePair<string, string>(".kfo", "application/vnd.kde.kformula"));
            _mimetypes.Add(new KeyValuePair<string, string>(".flw", "application/vnd.kde.kivio"));
            _mimetypes.Add(new KeyValuePair<string, string>(".kon", "application/vnd.kde.kontour"));
            _mimetypes.Add(new KeyValuePair<string, string>(".kpr", "application/vnd.kde.kpresenter"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ksp", "application/vnd.kde.kspread"));
            _mimetypes.Add(new KeyValuePair<string, string>(".kwd", "application/vnd.kde.kword"));
            _mimetypes.Add(new KeyValuePair<string, string>(".htke", "application/vnd.kenameaapp"));
            _mimetypes.Add(new KeyValuePair<string, string>(".kia", "application/vnd.kidspiration"));
            _mimetypes.Add(new KeyValuePair<string, string>(".kne", "application/vnd.kinar"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sse", "application/vnd.kodak-descriptor"));
            _mimetypes.Add(new KeyValuePair<string, string>(".lasxml", "application/vnd.las.las+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".latex", "application/x-latex"));
            _mimetypes.Add(new KeyValuePair<string, string>(".lbd", "application/vnd.llamagraphics.life-balance.desktop"));
            _mimetypes.Add(new KeyValuePair<string, string>(".lbe", "application/vnd.llamagraphics.life-balance.exchange+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".jam", "application/vnd.jam"));
            _mimetypes.Add(new KeyValuePair<string, string>("0.123", "application/vnd.lotus-1-2-3"));
            _mimetypes.Add(new KeyValuePair<string, string>(".apr", "application/vnd.lotus-approach"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pre", "application/vnd.lotus-freelance"));
            _mimetypes.Add(new KeyValuePair<string, string>(".nsf", "application/vnd.lotus-notes"));
            _mimetypes.Add(new KeyValuePair<string, string>(".org", "application/vnd.lotus-organizer"));
            _mimetypes.Add(new KeyValuePair<string, string>(".scm", "application/vnd.lotus-screencam"));
            _mimetypes.Add(new KeyValuePair<string, string>(".lwp", "application/vnd.lotus-wordpro"));
            _mimetypes.Add(new KeyValuePair<string, string>(".lvp", "audio/vnd.lucent.voice"));
            _mimetypes.Add(new KeyValuePair<string, string>(".m3u", "audio/x-mpegurl"));
            _mimetypes.Add(new KeyValuePair<string, string>(".m4v", "video/x-m4v"));
            _mimetypes.Add(new KeyValuePair<string, string>(".hqx", "application/mac-binhex40"));
            _mimetypes.Add(new KeyValuePair<string, string>(".portpkg", "application/vnd.macports.portpkg"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mgp", "application/vnd.osgeo.mapguide.package"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mrc", "application/marc"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mrcx", "application/marcxml+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mxf", "application/mxf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".nbp", "application/vnd.wolfram.player"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ma", "application/mathematica"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mathml", "application/mathml+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mbox", "application/mbox"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mc1", "application/vnd.medcalcdata"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mscml", "application/mediaservercontrol+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cdkey", "application/vnd.mediastation.cdkey"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mwf", "application/vnd.mfer"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mfm", "application/vnd.mfmp"));
            _mimetypes.Add(new KeyValuePair<string, string>(".msh", "model/mesh"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mads", "application/mads+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mets", "application/mets+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mods", "application/mods+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".meta4", "application/metalink4+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mcd", "application/vnd.mcd"));
            _mimetypes.Add(new KeyValuePair<string, string>(".flo", "application/vnd.micrografx.flo"));
            _mimetypes.Add(new KeyValuePair<string, string>(".igx", "application/vnd.micrografx.igx"));
            _mimetypes.Add(new KeyValuePair<string, string>(".es3", "application/vnd.eszigno3+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mdb", "application/x-msaccess"));
            _mimetypes.Add(new KeyValuePair<string, string>(".asf", "video/x-ms-asf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".exe", "application/x-msdownload"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cil", "application/vnd.ms-artgalry"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cab", "application/vnd.ms-cab-compressed"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ims", "application/vnd.ms-ims"));
            _mimetypes.Add(new KeyValuePair<string, string>(".application", "application/x-ms-application"));
            _mimetypes.Add(new KeyValuePair<string, string>(".clp", "application/x-msclip"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mdi", "image/vnd.ms-modi"));
            _mimetypes.Add(new KeyValuePair<string, string>(".eot", "application/vnd.ms-fontobject"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xls", "application/vnd.ms-excel"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xlam", "application/vnd.ms-excel.addin.macroenabled.12"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xlsb", "application/vnd.ms-excel.sheet.binary.macroenabled.12"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xltm", "application/vnd.ms-excel.template.macroenabled.12"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xlsm", "application/vnd.ms-excel.sheet.macroenabled.12"));
            _mimetypes.Add(new KeyValuePair<string, string>(".chm", "application/vnd.ms-htmlhelp"));
            _mimetypes.Add(new KeyValuePair<string, string>(".crd", "application/x-mscardfile"));
            _mimetypes.Add(new KeyValuePair<string, string>(".lrm", "application/vnd.ms-lrm"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mvb", "application/x-msmediaview"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mny", "application/x-msmoney"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow"));
            _mimetypes.Add(new KeyValuePair<string, string>(".potx", "application/vnd.openxmlformats-officedocument.presentationml.template"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template"));
            _mimetypes.Add(new KeyValuePair<string, string>(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"));
            _mimetypes.Add(new KeyValuePair<string, string>(".obd", "application/x-msbinder"));
            _mimetypes.Add(new KeyValuePair<string, string>(".thmx", "application/vnd.ms-officetheme"));
            _mimetypes.Add(new KeyValuePair<string, string>(".onetoc", "application/onenote"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pya", "audio/vnd.ms-playready.media.pya"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pyv", "video/vnd.ms-playready.media.pyv"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ppt", "application/vnd.ms-powerpoint"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ppam", "application/vnd.ms-powerpoint.addin.macroenabled.12"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sldm", "application/vnd.ms-powerpoint.slide.macroenabled.12"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pptm", "application/vnd.ms-powerpoint.presentation.macroenabled.12"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ppsm", "application/vnd.ms-powerpoint.slideshow.macroenabled.12"));
            _mimetypes.Add(new KeyValuePair<string, string>(".potm", "application/vnd.ms-powerpoint.template.macroenabled.12"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mpp", "application/vnd.ms-project"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pub", "application/x-mspublisher"));
            _mimetypes.Add(new KeyValuePair<string, string>(".scd", "application/x-msschedule"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xap", "application/x-silverlight-app"));
            _mimetypes.Add(new KeyValuePair<string, string>(".stl", "application/vnd.ms-pki.stl"));
            _mimetypes.Add(new KeyValuePair<string, string>(".cat", "application/vnd.ms-pki.seccat"));
            _mimetypes.Add(new KeyValuePair<string, string>(".vsd", "application/vnd.visio"));
            _mimetypes.Add(new KeyValuePair<string, string>(".vsdx", "application/vnd.visio2013"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wm", "video/x-ms-wm"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wma", "audio/x-ms-wma"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wax", "audio/x-ms-wax"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wmx", "video/x-ms-wmx"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wmd", "application/x-ms-wmd"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wpl", "application/vnd.ms-wpl"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wmz", "application/x-ms-wmz"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wmv", "video/x-ms-wmv"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wvx", "video/x-ms-wvx"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wmf", "application/x-msmetafile"));
            _mimetypes.Add(new KeyValuePair<string, string>(".trm", "application/x-msterminal"));
            _mimetypes.Add(new KeyValuePair<string, string>(".doc", "application/msword"));
            _mimetypes.Add(new KeyValuePair<string, string>(".docm", "application/vnd.ms-word.document.macroenabled.12"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dotm", "application/vnd.ms-word.template.macroenabled.12"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wri", "application/x-mswrite"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wps", "application/vnd.ms-works"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xbap", "application/x-ms-xbap"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xps", "application/vnd.ms-xpsdocument"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mid", "audio/midi"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mpy", "application/vnd.ibm.minipay"));
            _mimetypes.Add(new KeyValuePair<string, string>(".afp", "application/vnd.ibm.modcap"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rms", "application/vnd.jcp.javame.midlet-rms"));
            _mimetypes.Add(new KeyValuePair<string, string>(".tmo", "application/vnd.tmobile-livetv"));
            _mimetypes.Add(new KeyValuePair<string, string>(".prc", "application/x-mobipocket-ebook"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mbk", "application/vnd.mobius.mbk"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dis", "application/vnd.mobius.dis"));
            _mimetypes.Add(new KeyValuePair<string, string>(".plc", "application/vnd.mobius.plc"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mqy", "application/vnd.mobius.mqy"));
            _mimetypes.Add(new KeyValuePair<string, string>(".msl", "application/vnd.mobius.msl"));
            _mimetypes.Add(new KeyValuePair<string, string>(".txf", "application/vnd.mobius.txf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".daf", "application/vnd.mobius.daf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".fly", "text/vnd.fly"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mpc", "application/vnd.mophun.certificate"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mpn", "application/vnd.mophun.application"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mj2", "video/mj2"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mpga", "audio/mpeg"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mxu", "video/vnd.mpegurl"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mpeg", "video/mpeg"));
            _mimetypes.Add(new KeyValuePair<string, string>(".m21", "application/mp21"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mp4a", "audio/mp4"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mp4", "video/mp4"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mp4", "application/mp4"));
            _mimetypes.Add(new KeyValuePair<string, string>(".m3u8", "application/vnd.apple.mpegurl"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mus", "application/vnd.musician"));
            _mimetypes.Add(new KeyValuePair<string, string>(".msty", "application/vnd.muvee.style"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mxml", "application/xv+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ngdat", "application/vnd.nokia.n-gage.data"));
            _mimetypes.Add(new KeyValuePair<string, string>(".n-gage", "application/vnd.nokia.n-gage.symbian.install"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ncx", "application/x-dtbncx+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".nc", "application/x-netcdf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".nlu", "application/vnd.neurolanguage.nlu"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dna", "application/vnd.dna"));
            _mimetypes.Add(new KeyValuePair<string, string>(".nnd", "application/vnd.noblenet-directory"));
            _mimetypes.Add(new KeyValuePair<string, string>(".nns", "application/vnd.noblenet-sealer"));
            _mimetypes.Add(new KeyValuePair<string, string>(".nnw", "application/vnd.noblenet-web"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rpst", "application/vnd.nokia.radio-preset"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rpss", "application/vnd.nokia.radio-presets"));
            _mimetypes.Add(new KeyValuePair<string, string>(".n3", "text/n3"));
            _mimetypes.Add(new KeyValuePair<string, string>(".edm", "application/vnd.novadigm.edm"));
            _mimetypes.Add(new KeyValuePair<string, string>(".edx", "application/vnd.novadigm.edx"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ext", "application/vnd.novadigm.ext"));
            _mimetypes.Add(new KeyValuePair<string, string>(".gph", "application/vnd.flographit"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ecelp4800", "audio/vnd.nuera.ecelp4800"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ecelp7470", "audio/vnd.nuera.ecelp7470"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ecelp9600", "audio/vnd.nuera.ecelp9600"));
            _mimetypes.Add(new KeyValuePair<string, string>(".oda", "application/oda"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ogx", "application/ogg"));
            _mimetypes.Add(new KeyValuePair<string, string>(".oga", "audio/ogg"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ogv", "video/ogg"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dd2", "application/vnd.oma.dd2+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".oth", "application/vnd.oasis.opendocument.text-web"));
            _mimetypes.Add(new KeyValuePair<string, string>(".opf", "application/oebps-package+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".qbo", "application/vnd.intu.qbo"));
            _mimetypes.Add(new KeyValuePair<string, string>(".oxt", "application/vnd.openofficeorg.extension"));
            _mimetypes.Add(new KeyValuePair<string, string>(".osf", "application/vnd.yamaha.openscoreformat"));
            _mimetypes.Add(new KeyValuePair<string, string>(".weba", "audio/webm"));
            _mimetypes.Add(new KeyValuePair<string, string>(".webm", "video/webm"));
            _mimetypes.Add(new KeyValuePair<string, string>(".odc", "application/vnd.oasis.opendocument.chart"));
            _mimetypes.Add(new KeyValuePair<string, string>(".otc", "application/vnd.oasis.opendocument.chart-template"));
            _mimetypes.Add(new KeyValuePair<string, string>(".odb", "application/vnd.oasis.opendocument.database"));
            _mimetypes.Add(new KeyValuePair<string, string>(".odf", "application/vnd.oasis.opendocument.formula"));
            _mimetypes.Add(new KeyValuePair<string, string>(".odft", "application/vnd.oasis.opendocument.formula-template"));
            _mimetypes.Add(new KeyValuePair<string, string>(".odg", "application/vnd.oasis.opendocument.graphics"));
            _mimetypes.Add(new KeyValuePair<string, string>(".otg", "application/vnd.oasis.opendocument.graphics-template"));
            _mimetypes.Add(new KeyValuePair<string, string>(".odi", "application/vnd.oasis.opendocument.image"));
            _mimetypes.Add(new KeyValuePair<string, string>(".oti", "application/vnd.oasis.opendocument.image-template"));
            _mimetypes.Add(new KeyValuePair<string, string>(".odp", "application/vnd.oasis.opendocument.presentation"));
            _mimetypes.Add(new KeyValuePair<string, string>(".otp", "application/vnd.oasis.opendocument.presentation-template"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ods", "application/vnd.oasis.opendocument.spreadsheet"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ots", "application/vnd.oasis.opendocument.spreadsheet-template"));
            _mimetypes.Add(new KeyValuePair<string, string>(".odt", "application/vnd.oasis.opendocument.text"));
            _mimetypes.Add(new KeyValuePair<string, string>(".odm", "application/vnd.oasis.opendocument.text-master"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ott", "application/vnd.oasis.opendocument.text-template"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ktx", "image/ktx"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sxc", "application/vnd.sun.xml.calc"));
            _mimetypes.Add(new KeyValuePair<string, string>(".stc", "application/vnd.sun.xml.calc.template"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sxd", "application/vnd.sun.xml.draw"));
            _mimetypes.Add(new KeyValuePair<string, string>(".std", "application/vnd.sun.xml.draw.template"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sxi", "application/vnd.sun.xml.impress"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sti", "application/vnd.sun.xml.impress.template"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sxm", "application/vnd.sun.xml.math"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sxw", "application/vnd.sun.xml.writer"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sxg", "application/vnd.sun.xml.writer.global"));
            _mimetypes.Add(new KeyValuePair<string, string>(".stw", "application/vnd.sun.xml.writer.template"));
            _mimetypes.Add(new KeyValuePair<string, string>(".otf", "application/x-font-otf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".osfpvg", "application/vnd.yamaha.openscoreformat.osfpvg+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dp", "application/vnd.osgi.dp"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pdb", "application/vnd.palm"));
            _mimetypes.Add(new KeyValuePair<string, string>(".p", "text/x-pascal"));
            _mimetypes.Add(new KeyValuePair<string, string>(".paw", "application/vnd.pawaafile"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pclxl", "application/vnd.hp-pclxl"));
            _mimetypes.Add(new KeyValuePair<string, string>(".efif", "application/vnd.picsel"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pcx", "image/x-pcx"));
            _mimetypes.Add(new KeyValuePair<string, string>(".psd", "image/vnd.adobe.photoshop"));
            _mimetypes.Add(new KeyValuePair<string, string>(".prf", "application/pics-rules"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pic", "image/x-pict"));
            _mimetypes.Add(new KeyValuePair<string, string>(".chat", "application/x-chat"));
            _mimetypes.Add(new KeyValuePair<string, string>(".p10", "application/pkcs10"));
            _mimetypes.Add(new KeyValuePair<string, string>(".p12", "application/x-pkcs12"));
            _mimetypes.Add(new KeyValuePair<string, string>(".p7m", "application/pkcs7-mime"));
            _mimetypes.Add(new KeyValuePair<string, string>(".p7s", "application/pkcs7-signature"));
            _mimetypes.Add(new KeyValuePair<string, string>(".p7r", "application/x-pkcs7-certreqresp"));
            _mimetypes.Add(new KeyValuePair<string, string>(".p7b", "application/x-pkcs7-certificates"));
            _mimetypes.Add(new KeyValuePair<string, string>(".p8", "application/pkcs8"));
            _mimetypes.Add(new KeyValuePair<string, string>(".plf", "application/vnd.pocketlearn"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pnm", "image/x-portable-anymap"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pbm", "image/x-portable-bitmap"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pcf", "application/x-font-pcf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pfr", "application/font-tdpfr"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pgn", "application/x-chess-pgn"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pgm", "image/x-portable-graymap"));
            _mimetypes.Add(new KeyValuePair<string, string>(".png", "image/png"));
            _mimetypes.Add(new KeyValuePair<string, string>(".png", "image/x-citrix-png"));
            _mimetypes.Add(new KeyValuePair<string, string>(".png", "image/x-png"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ppm", "image/x-portable-pixmap"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pskcxml", "application/pskc+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pml", "application/vnd.ctc-posml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ai", "application/postscript"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pfa", "application/x-font-type1"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pbd", "application/vnd.powerbuilder6"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pgp", "application/pgp-encrypted"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pgp", "application/pgp-signature"));
            _mimetypes.Add(new KeyValuePair<string, string>(".box", "application/vnd.previewsystems.box"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ptid", "application/vnd.pvi.ptid1"));
            _mimetypes.Add(new KeyValuePair<string, string>(".pls", "application/pls+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".str", "application/vnd.pg.format"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ei6", "application/vnd.pg.osasli"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dsc", "text/prs.lines.tag"));
            _mimetypes.Add(new KeyValuePair<string, string>(".psf", "application/x-font-linux-psf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".qps", "application/vnd.publishare-delta-tree"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wg", "application/vnd.pmi.widget"));
            _mimetypes.Add(new KeyValuePair<string, string>(".qxd", "application/vnd.quark.quarkxpress"));
            _mimetypes.Add(new KeyValuePair<string, string>(".esf", "application/vnd.epson.esf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".msf", "application/vnd.epson.msf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ssf", "application/vnd.epson.ssf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".qam", "application/vnd.epson.quickanime"));
            _mimetypes.Add(new KeyValuePair<string, string>(".qfx", "application/vnd.intu.qfx"));
            _mimetypes.Add(new KeyValuePair<string, string>(".qt", "video/quicktime"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rar", "application/x-rar-compressed"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ram", "audio/x-pn-realaudio"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rmp", "audio/x-pn-realaudio-plugin"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rsd", "application/rsd+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rm", "application/vnd.rn-realmedia"));
            _mimetypes.Add(new KeyValuePair<string, string>(".bed", "application/vnd.realvnc.bed"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mxl", "application/vnd.recordare.musicxml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".musicxml", "application/vnd.recordare.musicxml+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rnc", "application/relax-ng-compact-syntax"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rdz", "application/vnd.data-vision.rdz"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rdf", "application/rdf+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rp9", "application/vnd.cloanto.rp9"));
            _mimetypes.Add(new KeyValuePair<string, string>(".jisp", "application/vnd.jisp"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rtf", "application/rtf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rtx", "text/richtext"));
            _mimetypes.Add(new KeyValuePair<string, string>(".link66", "application/vnd.route66.link66+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rss, .xml", "application/rss+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".shf", "application/shf+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".st", "application/vnd.sailingtracker.track"));
            _mimetypes.Add(new KeyValuePair<string, string>(".svg", "image/svg+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sus", "application/vnd.sus-calendar"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sru", "application/sru+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".setpay", "application/set-payment-initiation"));
            _mimetypes.Add(new KeyValuePair<string, string>(".setreg", "application/set-registration-initiation"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sema", "application/vnd.sema"));
            _mimetypes.Add(new KeyValuePair<string, string>(".semd", "application/vnd.semd"));
            _mimetypes.Add(new KeyValuePair<string, string>(".semf", "application/vnd.semf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".see", "application/vnd.seemail"));
            _mimetypes.Add(new KeyValuePair<string, string>(".snf", "application/x-font-snf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".spq", "application/scvp-vp-request"));
            _mimetypes.Add(new KeyValuePair<string, string>(".spp", "application/scvp-vp-response"));
            _mimetypes.Add(new KeyValuePair<string, string>(".scq", "application/scvp-cv-request"));
            _mimetypes.Add(new KeyValuePair<string, string>(".scs", "application/scvp-cv-response"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sdp", "application/sdp"));
            _mimetypes.Add(new KeyValuePair<string, string>(".etx", "text/x-setext"));
            _mimetypes.Add(new KeyValuePair<string, string>(".movie", "video/x-sgi-movie"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ifm", "application/vnd.shana.informed.formdata"));
            _mimetypes.Add(new KeyValuePair<string, string>(".itp", "application/vnd.shana.informed.formtemplate"));
            _mimetypes.Add(new KeyValuePair<string, string>(".iif", "application/vnd.shana.informed.interchange"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ipk", "application/vnd.shana.informed.package"));
            _mimetypes.Add(new KeyValuePair<string, string>(".tfi", "application/thraud+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".shar", "application/x-shar"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rgb", "image/x-rgb"));
            _mimetypes.Add(new KeyValuePair<string, string>(".slt", "application/vnd.epson.salt"));
            _mimetypes.Add(new KeyValuePair<string, string>(".aso", "application/vnd.accpac.simply.aso"));
            _mimetypes.Add(new KeyValuePair<string, string>(".imp", "application/vnd.accpac.simply.imp"));
            _mimetypes.Add(new KeyValuePair<string, string>(".twd", "application/vnd.simtech-mindmapper"));
            _mimetypes.Add(new KeyValuePair<string, string>(".csp", "application/vnd.commonspace"));
            _mimetypes.Add(new KeyValuePair<string, string>(".saf", "application/vnd.yamaha.smaf-audio"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mmf", "application/vnd.smaf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".spf", "application/vnd.yamaha.smaf-phrase"));
            _mimetypes.Add(new KeyValuePair<string, string>(".teacher", "application/vnd.smart.teacher"));
            _mimetypes.Add(new KeyValuePair<string, string>(".svd", "application/vnd.svd"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rq", "application/sparql-query"));
            _mimetypes.Add(new KeyValuePair<string, string>(".srx", "application/sparql-results+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".gram", "application/srgs"));
            _mimetypes.Add(new KeyValuePair<string, string>(".grxml", "application/srgs+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ssml", "application/ssml+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".skp", "application/vnd.koan"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sgml", "text/sgml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sdc", "application/vnd.stardivision.calc"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sda", "application/vnd.stardivision.draw"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sdd", "application/vnd.stardivision.impress"));
            _mimetypes.Add(new KeyValuePair<string, string>(".smf", "application/vnd.stardivision.math"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sdw", "application/vnd.stardivision.writer"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sgl", "application/vnd.stardivision.writer-global"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sm", "application/vnd.stepmania.stepchart"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sit", "application/x-stuffit"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sitx", "application/x-stuffitx"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sdkm", "application/vnd.solent.sdkm+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xo", "application/vnd.olpc-sugar"));
            _mimetypes.Add(new KeyValuePair<string, string>(".au", "audio/basic"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wqd", "application/vnd.wqd"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sis", "application/vnd.symbian.install"));
            _mimetypes.Add(new KeyValuePair<string, string>(".smi", "application/smil+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xsm", "application/vnd.syncml+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".bdm", "application/vnd.syncml.dm+wbxml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xdm", "application/vnd.syncml.dm+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sv4cpio", "application/x-sv4cpio"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sv4crc", "application/x-sv4crc"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sbml", "application/sbml+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".tsv", "text/tab-separated-values"));
            _mimetypes.Add(new KeyValuePair<string, string>(".tiff", "image/tiff"));
            _mimetypes.Add(new KeyValuePair<string, string>(".tao", "application/vnd.tao.intent-module-archive"));
            _mimetypes.Add(new KeyValuePair<string, string>(".tar", "application/x-tar"));
            _mimetypes.Add(new KeyValuePair<string, string>(".tcl", "application/x-tcl"));
            _mimetypes.Add(new KeyValuePair<string, string>(".tex", "application/x-tex"));
            _mimetypes.Add(new KeyValuePair<string, string>(".tfm", "application/x-tex-tfm"));
            _mimetypes.Add(new KeyValuePair<string, string>(".tei", "application/tei+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".txt", "text/plain"));
            _mimetypes.Add(new KeyValuePair<string, string>(".dxp", "application/vnd.spotfire.dxp"));
            _mimetypes.Add(new KeyValuePair<string, string>(".sfs", "application/vnd.spotfire.sfs"));
            _mimetypes.Add(new KeyValuePair<string, string>(".tsd", "application/timestamped-data"));
            _mimetypes.Add(new KeyValuePair<string, string>(".tpt", "application/vnd.trid.tpt"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mxs", "application/vnd.triscape.mxs"));
            _mimetypes.Add(new KeyValuePair<string, string>(".t", "text/troff"));
            _mimetypes.Add(new KeyValuePair<string, string>(".tra", "application/vnd.trueapp"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ttf", "application/x-font-ttf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ttl", "text/turtle"));
            _mimetypes.Add(new KeyValuePair<string, string>(".umj", "application/vnd.umajin"));
            _mimetypes.Add(new KeyValuePair<string, string>(".uoml", "application/vnd.uoml+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".unityweb", "application/vnd.unity"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ufd", "application/vnd.ufdl"));
            _mimetypes.Add(new KeyValuePair<string, string>(".uri", "text/uri-list"));
            _mimetypes.Add(new KeyValuePair<string, string>(".utz", "application/vnd.uiq.theme"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ustar", "application/x-ustar"));
            _mimetypes.Add(new KeyValuePair<string, string>(".uu", "text/x-uuencode"));
            _mimetypes.Add(new KeyValuePair<string, string>(".vcs", "text/x-vcalendar"));
            _mimetypes.Add(new KeyValuePair<string, string>(".vcf", "text/x-vcard"));
            _mimetypes.Add(new KeyValuePair<string, string>(".vcd", "application/x-cdlink"));
            _mimetypes.Add(new KeyValuePair<string, string>(".vsf", "application/vnd.vsf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wrl", "model/vrml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".vcx", "application/vnd.vcx"));
            _mimetypes.Add(new KeyValuePair<string, string>(".mts", "model/vnd.mts"));
            _mimetypes.Add(new KeyValuePair<string, string>(".vtu", "model/vnd.vtu"));
            _mimetypes.Add(new KeyValuePair<string, string>(".vis", "application/vnd.visionary"));
            _mimetypes.Add(new KeyValuePair<string, string>(".viv", "video/vnd.vivo"));
            _mimetypes.Add(new KeyValuePair<string, string>(".ccxml", "application/ccxml+xml,"));
            _mimetypes.Add(new KeyValuePair<string, string>(".vxml", "application/voicexml+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".src", "application/x-wais-source"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wbxml", "application/vnd.wap.wbxml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wbmp", "image/vnd.wap.wbmp"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wav", "audio/x-wav"));
            _mimetypes.Add(new KeyValuePair<string, string>(".davmount", "application/davmount+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".woff", "application/x-font-woff"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wspolicy", "application/wspolicy+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".webp", "image/webp"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wtb", "application/vnd.webturbo"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wgt", "application/widget"));
            _mimetypes.Add(new KeyValuePair<string, string>(".hlp", "application/winhlp"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wml", "text/vnd.wap.wml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wmls", "text/vnd.wap.wmlscript"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wmlsc", "application/vnd.wap.wmlscriptc"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wpd", "application/vnd.wordperfect"));
            _mimetypes.Add(new KeyValuePair<string, string>(".stf", "application/vnd.wt.stf"));
            _mimetypes.Add(new KeyValuePair<string, string>(".wsdl", "application/wsdl+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xbm", "image/x-xbitmap"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xpm", "image/x-xpixmap"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xwd", "image/x-xwindowdump"));
            _mimetypes.Add(new KeyValuePair<string, string>(".der", "application/x-x509-ca-cert"));
            _mimetypes.Add(new KeyValuePair<string, string>(".fig", "application/x-xfig"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xhtml", "application/xhtml+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xml", "application/xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xdf", "application/xcap-diff+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xenc", "application/xenc+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xer", "application/patch-ops-error+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rl", "application/resource-lists+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rs", "application/rls-services+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".rld", "application/resource-lists-diff+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xslt", "application/xslt+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xop", "application/xop+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xpi", "application/x-xpinstall"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xspf", "application/xspf+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xul", "application/vnd.mozilla.xul+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".xyz", "chemical/x-xyz"));
            _mimetypes.Add(new KeyValuePair<string, string>(".yaml", "text/yaml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".yang", "application/yang"));
            _mimetypes.Add(new KeyValuePair<string, string>(".yin", "application/yin+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".zir", "application/vnd.zul"));
            _mimetypes.Add(new KeyValuePair<string, string>(".zip", "application/zip"));
            _mimetypes.Add(new KeyValuePair<string, string>(".zmm", "application/vnd.handheld-entertainment+xml"));
            _mimetypes.Add(new KeyValuePair<string, string>(".zaz", "application/vnd.zzazz.deck+xml"));
            #endregion
        }
        public FileManager()
        {
            
        }

        public FileManager(string LocalOrRemoteFile)
        {
            try
            {
                MimeTypeInitialization();
                byte[] fileData = null;
                if (LocalOrRemoteFile.ToLower().StartsWith("http"))
                {
                    using (var webClient = new WebClient())
                    {
                        fileData = webClient.DownloadData(LocalOrRemoteFile);
                    }
                }
                else
                {

                    using (FileStream fs = File.OpenRead(LocalOrRemoteFile))
                    {
                        var binaryReader = new BinaryReader(fs);
                        fileData = binaryReader.ReadBytes((int)fs.Length);
                    }
                }

                this.FileBytes = fileData;
                this.Name = Path.GetFileNameWithoutExtension(LocalOrRemoteFile);
                this.Extension = Path.GetExtension(LocalOrRemoteFile);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public FileManager(string LocalOrRemoteFile,string customName)
        {
            try
            {
                MimeTypeInitialization();
                byte[] fileData = null;
                if (LocalOrRemoteFile.ToLower().StartsWith("http"))
                {
                    using (var webClient = new WebClient())
                    {
                        fileData = webClient.DownloadData(LocalOrRemoteFile);
                    }
                }
                else
                {

                    using (FileStream fs = File.OpenRead(HelpingClass.GetDefaultDirectory() +LocalOrRemoteFile.Replace("/","\\")))
                    {
                        var binaryReader = new BinaryReader(fs);
                        fileData = binaryReader.ReadBytes((int)fs.Length);
                    }
                }

                this.FileBytes = fileData;
                this.Name = customName;
                this.Extension = Path.GetExtension(LocalOrRemoteFile);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ByteArrayToFile(FileManager fm, string directory, out string savedfilepath)
        {
            try
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (File.Exists(directory + "\\" + fm.Name + fm.Extension))
                {
                    File.Delete(directory + "\\" + fm.Name + fm.Extension);
                }

                using (var fs = new FileStream(directory+"\\" + fm.Name + fm.Extension, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(fm.FileBytes, 0, fm.FileBytes.Length);
                    savedfilepath = directory + "\\" + fm.Name + fm.Extension;
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static byte[] InputStreamToByteArray(System.IO.Stream stream)
        {
            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);

            return ms.ToArray();
        }
    }
}