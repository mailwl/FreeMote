﻿using System;
using System.IO;
using FreeMote.Psb;
using FreeMote.PsBuild;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FreeMote.Tests
{
    [TestClass]
    public class MmoTest
    {
        public MmoTest()
        {
        }

        //nekomimi is binded to eyebrow
        //1:gif 2:trialable
        //1:free 2:academy 3:indies 4:commercial 5:common
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        public void TestPackMmo()
        {
            var resPath = Path.Combine(Environment.CurrentDirectory, @"..\..\Res");
            var path = Path.Combine(resPath, "template39.json");
            var path2 = Path.Combine(resPath, "template39-krkr.json");
            var psb = PsbCompiler.LoadPsbFromJsonFile(path);
            //var psb2 = PsbCompiler.LoadPsbFromJsonFile(path2);
            //psb.Objects["objectChildren"] = psb2.Objects["object"];
            //var collection = (PsbCollection)psb.Objects["objectChildren"];
            //collection.RemoveAt(0);
            psb.Objects["metaformat"] = PsbNull.Null;
            psb.Merge();
            psb.SaveAsMdfFile("temp.mmo");
        }

        [TestMethod]
        public void TestLoadMmo()
        {
            var resPath = Path.Combine(Environment.CurrentDirectory, @"..\..\Res");
            var path = Path.Combine(resPath, "mmo", "template39.mmo");
            var psb = new PSB(path);
            var content = (PsbDictionary)psb.Objects.FindByPath("objectChildren/[0]/children/[0]/layerChildren/[0]/frameList/[0]/content");
            foreach (var kv in content)
            {
                var k = kv.Key;
                var v = kv.Value;
            }
        }

        [TestMethod]
        public void TestMmoGraft()
        {
            var resPath = Path.Combine(Environment.CurrentDirectory, @"..\..\Res");
            var path = Path.Combine(resPath, "template39.json");
            var path2 = Path.Combine(resPath, "template39-krkr.json");
            var mmo = PsbCompiler.LoadPsbFromJsonFile(path);
            var psb = PsbCompiler.LoadPsbFromJsonFile(path2);
            MmoBuilder mmoBuilder = new MmoBuilder(true);
            var psbMmo = mmoBuilder.Build(psb);
            //mmo.Objects["objectChildren"] = psbMmo.Objects["objectChildren"];
            var data = (PsbDictionary)mmo.Objects["metaformat"].Children("data");
            var data2 = (PsbDictionary)psbMmo.Objects["metaformat"].Children("data");
            data["bustControlDefinitionList"] = data2["bustControlDefinitionList"];
            mmo.Merge();
            mmo.SaveAsMdfFile(Path.Combine(resPath, "mmo", "temp.mmo"));

        }

        [TestMethod]
        public void TestMmoGraft2()
        {
            var resPath = Path.Combine(Environment.CurrentDirectory, @"..\..\Res");
            var path = Path.Combine(resPath, "template39.json");
            var path2 = Path.Combine(resPath, "template39-krkr.json");
            //var path = Path.Combine(resPath, "e-mote38基本テンプレート(正面zバイナリ専用)_free.json");
            //var path2 = Path.Combine(resPath, "e-mote3.0ショコラパジャマa中-krkr.json");
            //var path2 = Path.Combine(resPath, "mmo", "e-mote38基本テンプレート(正面zバイナリ専用)-krkr.json");
            var mmo = PsbCompiler.LoadPsbFromJsonFile(path);
            var psb = PsbCompiler.LoadPsbFromJsonFile(path2);
            MmoBuilder mmoBuilder = new MmoBuilder(true);
            var psbMmo = mmoBuilder.Build(psb);
            var pMd = (PsbDictionary)psbMmo.Objects["metaformat"].Children("data");
            var mMd = (PsbDictionary)mmo.Objects["metaformat"].Children("data");
            ///*
            pMd["textureDefinitionList"] = mMd["textureDefinitionList"];
            pMd["scrapbookDefinitionList"] = mMd["scrapbookDefinitionList"];
            pMd["partsList"] = mMd["partsList"];
            pMd["layoutDefinitionList"] = mMd["layoutDefinitionList"];
            pMd["customPartsBaseDefinitionList"] = mMd["customPartsBaseDefinitionList"];
            pMd["customPartsMountDefinitionList"] = mMd["customPartsMountDefinitionList"];
            pMd["customPartsCount"] = mMd["customPartsCount"];
            pMd["sourceDefinitionOrderList"] = mMd["sourceDefinitionOrderList"];
            pMd["charaProfileDefinitionList"] = mMd["charaProfileDefinitionList"];
            //*/
            //pMd["textureDefinitionList"] = new PsbCollection();
            /*
            psbMmo.Objects["metaformat"] = mmo.Objects["metaformat"];
            mMd["textureDefinitionList"] = pMd["textureDefinitionList"];
            mMd["scrapbookDefinitionList"] = pMd["scrapbookDefinitionList"];
            */
            psbMmo.Merge();
            File.WriteAllBytes(Path.Combine(resPath, "mmo", "crash-temp.mmo"), psbMmo.Build());
        }

        [TestMethod]
        public void TestBuildMmo()
        {
            var resPath = Path.Combine(Environment.CurrentDirectory, @"..\..\Res");
            var path = Path.Combine(resPath, "e-mote3.0ショコラパジャマa中-krkr.json");
            //var path = Path.Combine(resPath, "template39-krkr.json");
            var psb = PsbCompiler.LoadPsbFromJsonFile(path);
            MmoBuilder mmoBuilder = new MmoBuilder(true);
            var psbMmo = mmoBuilder.Build(psb);
            psbMmo.Merge();
            File.WriteAllBytes(Path.Combine(resPath, "mmo", "NekoCrash.mmo"), psbMmo.Build());
        }

        [TestMethod]
        public void TestFindPath()
        {
            var resPath = Path.Combine(Environment.CurrentDirectory, @"..\..\Res");
            //var path = Path.Combine(resPath, "template39.json");
            var path = Path.Combine(resPath, "mmo", "NekoCrash.json");
            var mmo = PsbCompiler.LoadPsbFromJsonFile(path);

            var children = (PsbCollection)mmo.Objects["objectChildren"];
            var source = (PsbCollection)mmo.Objects["sourceChildren"];
            var obj = (PsbDictionary)children.FindByMmoPath(
                "all_parts/全体構造/■全体レイアウト/move_UD/move_LR/□下半身配置_le/胴体回転中心/全身調整/□頭部調整_le/act_sp");
            var realPath = obj.Path;
            var mmoPath = obj.GetMmoPath(); //"FreeMote/all_parts/全体構造/■全体レイアウト/move_UD/move_LR/□下半身配置_le/胴体回転中心/全身調整/□頭部調整_le/act_sp"
            //obj = source.FindByMmoPath("face_eye_mabuta_l");
        }

        [TestMethod]
        public void TestCompareMmo()
        {
            var resPath = Path.Combine(Environment.CurrentDirectory, @"..\..\Res\mmo");
            var path = Path.Combine(resPath, "template39.json");
            var path2 = Path.Combine(resPath, "crash-temp.mmo");

            var mmo1 = PsbCompiler.LoadPsbFromJsonFile(path);
            var allpart1 = FindPart((PsbCollection)mmo1.Objects["objectChildren"], "body_parts");
            var mmo2 = new PSB(path2);
            var allpart2 = FindPart((PsbCollection)mmo2.Objects["objectChildren"], "body_parts");

            //var p1 = mmo1.Objects.FindByPath(
            //    "/objectChildren/[3]/children/[1]/layerChildren/[0]/children/[0]/frameList/[0]/content/coord");
            //var pp = ((IPsbChild) p1).Parent.Parent.Parent.Parent["label"];
            PsbDictionary FindPart(PsbCollection col, string label)
            {
                foreach (var c in col)
                {
                    if (c is PsbDictionary d)
                    {
                        if (d["label"] is PsbString s && s.Value == label)
                        {
                            return d;
                        }
                    }
                }

                return null;
            }
            //PsBuildTest.CompareValue(allpart1, allpart2);
            PsBuildTest.CompareValue(mmo1.Objects["metaformat"].Children("data"), mmo2.Objects["metaformat"].Children("data"));
        }
    }
}
