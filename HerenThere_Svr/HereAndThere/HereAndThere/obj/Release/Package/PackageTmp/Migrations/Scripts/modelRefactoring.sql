﻿
ALTER TABLE [dbo].[Match] ADD [startTime] [datetime] NOT NULL DEFAULT '1900-01-01T00:00:00.000'
ALTER TABLE [dbo].[Match] ADD [endTime] [datetime]
DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Match')
AND col_name(parent_object_id, parent_column_id) = 'started';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Match] DROP CONSTRAINT [' + @var0 + ']')
ALTER TABLE [dbo].[Match] DROP COLUMN [started]
DECLARE @var1 nvarchar(128)
SELECT @var1 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Match')
AND col_name(parent_object_id, parent_column_id) = 'ended';
IF @var1 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Match] DROP CONSTRAINT [' + @var1 + ']')
ALTER TABLE [dbo].[Match] DROP COLUMN [ended]
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'201611031805061_ModelRefactoring', N'HereAndThere.Migrations.Configuration',  0x1F8B0800000000000400ED5DCD6EE4B811BE07C83B083A2581D76D7B83C5C668EFC263CF648D8CC7C6B43DC8CDA025BA2D44A27A25B66123C893ED218F945708F5CF5F8994D492DA2BEC61C7FC29168B551FC92A76E97FBFFD77F9F36BE05B2F308ABD109DD9C78747B6059113BA1E5A9FD95BFCF4DD8FF6CF3FFDF10FCB8F6EF06A7D2BDA7D9FB4233D517C663F63BC395D2C62E71906203E0C3C270AE3F0091F3A61B0006EB838393AFADBE2F8780109099BD0B2ACE5D72DC25E00D33FC89F172172E0066F817F1DBAD08FF37252B34AA95A5F4000E30D70E099FD0B8CE03972EF9EC9FF0FB3E6B675EE7B80B0B282FE936D0184420C3061F4F43E862B1C8568BDDA9002E0DFBD6D2069F704FC18E61338AD9AEBCEE5E82499CBA2EA589072B6310E034382C7DFE7C259F0DD5B89D82E8547C4F7918819BF25B34E4578667F08B7C805D19B6DF1839D5EF851D2502AE1C3A2DF8145D71E941A411427F9EFC0BAD8FA781BC13304B73802FE8175BB7DF43DE71FF0ED2EFC17446768EBFB348B844952C71490A2DB28DCC008BF7D854F39E39E6B5B0BB6DF82EF5876A3FA6453BA42F887BFDAD617323878F461A901D4F457388CE0DF218211C0D0BD0518C3882CE0950B53190AA3736305003BCF578D03D613F1092B78EBC282CA2574BC00F8B6751B917FE526FAA36DAD1C90D03C311F8098C26E4748CC7A8541B0294720E2BC2385C6949C08262BF1E1ADA0442C99A0926D5D83D7CF10ADF133C121C2E027EF15BA45414EF91E7904C31276A2ADF9C80181BF270FBA77EAB9E811E883777AA4E5A232E85A33FF1C3A3982989979D16F36F3F76BA15E7CEE245B6D31C08730F421402DE87C23EC90F65D0905E10B0C88F8BBA2E70C3D3B841E32D217F0E2AD532B522CA06D7D857EDA207EF636D9718C204B56F9E0E7D8428E6B9FA230F81A2642116B1FEE40B48684D65DA86CB20AB791D3161BAF4B6ECDB0B1E83763A362AC8D0FDE60D4D58A7B3948793159AD54EBBB4153A1723334ED293451A0D3073615C053834D057C698367A2EF72EE929A87025C59D6D82A1133B97A1960D6319599B294ABDBB44ACE165F27F02534E886E499E80C613CE93463781DFA26837545BC18830867A8D011F12072E57466C89D24E43E66AE220F2A30378526BA110F6B559D88B77C8356689B4842CD5B52FB90B693304757CA519769618ABB14AA76DB10E48213815997B1D821B022E76A95543DE4DB58C511552C088AAEEBBE0364CBD9621748AAE79D4031160215E60E06782E8C9DC8DB64AE2BE5D8E49F6D106BDE20A6B14194D8DA1D81A53027C5E856F092E29429B4A49D665851295C1FD7FB7E3C0DF126C4C9147A21B609BDC4A5B4239FED8C8BBF175C6C7BC4E2915076FCEAE1F69F912D1AF0AC64E50A5EF24AE393276DA5353C71ED78D6986A05876C9B4E3B47C186D9D691F59AF70EC558A31C4933B5EDC3253203EB58C05A7BAB367268F2C0A1F4789A61ADD223715B56E7F829618FAE55785C9926A64E89C6BB7FC37620BFFD737B851933E76BE0A1581E706CB11DC839946F191DB683368E8AAAE7BC2D4C695B984FE4BF838DA384DC3E9059BE7548C15B0763CEE33874BC942BEE610315CD64E7F911B956736833932D1D1E252226D0E26D08981046C855D6E641E3065D421F62689D3BD9BBD70B103BC015454D26E31A30550273C554F5B48D65EA2FC25804C10850A2E4DDEF05214630915CD145B8F390E36D80DF2819AEA7265426732EC7E06B2EE106A204E51A25A03338FD8E4A64A21C8B5B9026292D1794A63528201FDA51AEB432CE43295F76DB1D46F35461258A9FEAE5F46EF44E219221B44E317B2D9D2BDC79E3291CE32DAE5D61B9EB9853B9EC90369CDAC9238A0D76D0A7D2C9A43294D6C926AFAD76C59D7C44ACAB2E8DF5D022B9408E897492073ACDBB7EAF40270864309C13E63E7D98A3DDBFAA8595FA82AB45CD23594D2B2AA7A78B4ABDEA6C2BE592703D806249643F7DA512DC5E2A4D50FBC02A7528DC2C836098FA95E16020A692C900CAA69ABDCED055DC74449563AECCF56B2CBF3FF36A37E089ADCEE1DA640DBDAA9E4C3283299F6CFEFAEA37EAB18D7158D7EF7D7C30B3F366CABBC39BE1B31785910D3ED8A6C80A710F304A163DA85F564564B9B3BA284214836EBA1D144ECAFE607A275D149DD1854741036861E6E0257D30E901A39C133ADA73F998D4C257D98FD6EE63984790E2DCBFCDEB55427D05B1E47D76E55A16BC5C8276B254A85FCC08442A176D03112AE62A10A94E704D448A87822289EC92A2D33F159F8A4476C068205344490512B9F937742F630842FFC2CCB508A8E6419F943842945256D464BF8CA21AAA7F3FC55B8B56CCA19C0BA31282E569850A285A14EF3CD2B193D61188F0B30589386AFDDF7A1E705A148562D70942E5B9A6E8D04CF72307F681AE42106ABFACA667961745AEDB4DD2903A549BC5DA5A27288D55A984E292ADE5286CA510E2DD58C7C45A88807999284E5FE9B96AF45D51FC16C85A336B99B76A274B2E3E1612275DEF59D1F3AD50CC973B43CDFC952E911D2DBB2CF8AD1284FABEAF7BE31784D18C043517F566D1B6B683E22194CA1064F7CEE69B671B53E0EE9A3B9C33F7D04A35F59A1B95F69DAA8D20E4B7A84EF2285E649407F6B26EB9C8F295E505CB8522B1D9F21A6C361E5A5389CEF2126B956539BBF86E659EFD2BC8682C1CC6CEF8EB4539120E23B0865C2D199A70FAC98B627C09307804C9F3990B37109AC9AF278A336A31A670031197AF38B8165D927F8B17223E2F99E46E9713F8442699C05E3A5F482DBCBAAB95E49D033E88248FDA2E427F1B20F54D55DDBB74F7D324943100359D2A05114DA82A35A054E51A624855C5FAB4A8376A342DAA589F16F54A8DA645151B485D7C77C6C85FAC36A7CD334A978BD4960B4E330507836004829B86352B2DA3ABBBA299D95C79BD37B73975D7DDD8DC346DA5CAC0C5CCAC2C35A154E6E0624995C526DA5C3D1F63B559FDAC4C4D6DC68389E341DDF5C40C0F4A4F9D391EA8BBEE060FAAE0024D431D72A859EB9E76F32A53156BC045A9094255B9AA5888AACA67FBD5A1BD1FF6ABF4AC195AAFCC39A363BAF27E3B3C3B178163C1E2541165353D2A3B114D8D2AD6A755E627A2299585B3C5E9D0DE1F8BCB5C4FBD185D1A8D696978F2BEBB313E0478EDCE4AF429303F96A2093115B3A9E8D0DE0B53C9DD735DCD240B9D9A9B88A2DFB4FD3AFD9D4D85370CCC1ED7F4C0A186C32CC709C35E563443C10C050A2850477ECCB0401E39D0000355C7A96E96EC2B49110BCC4FBBB38DEC858DF473B4A45EFAB4B595F970395BCBE8D6224459F926E5E865B4958BAA2EF30867F337A5849067D624C9DF16BE786E12EE5CBDC51806874983C3D5AFFE85EFA56F138B06D700794F30C659A60DFBE4E8F884FB2AD574BE10B58863D7974488A97C25F2D8E810E9421EBD757AA26CCC09D2EDB302C530C26BDA2BE4C2D733FBDF69B753EBEA9F0F79CF03EB26220B7D6A1D59FFE9FC612777E79F8DE97D042109894B1606F7928404BD80C879069190CAA3AF1C2332567512AF8839473458EDE3B34DEFC6ECDE81E2F3DF4B7AF470F76F25B521227E27C904C5CACE9D806C4681DDA3803C28BACF28C0E7EB35D0DBA26B27AD1D77F3E73F46D4C6F6C50F111593980D787A062CC9D4B0C7D62BF9268CA90565BD3B5991F04199D68ACB7D50A6AD56CD9634902589EEA87DB6A6CCBDD5EF3A4B925116A4FF1480D73FCFAA3D3DD596C428F759ADC73D628D7DC0547C4AC2800D8E423769D0DFA2E8FD3E3C83CDFE818D3491C51EA38D7C13559BD9557C8FBC5FB7C4D4EE88101393EAA610B28F0218634E0F87E2D97686B29DF9103AEF0B7BAFDBBDE40C1F334578955986E261A08CE043242FAAF9699D38D69EE5FB1E2DE76D95D38862608874DE43684CCD0FA0251AB36FD9BAC74FCF3D7C2EEEC15221BFC74CDBE325D636D91DF70D648CB6C0E9E7CA1E5753F27C70D4E89A0922A7AD23AADF6A4C4D410CF35E8F97E67A0C44513EB21F1752F6316FF51412558F91957AB834D4EF2AE7B401DAF00B36D6D6321C58E8EF2D7B943D7AECFD655696E9E47A1673AEF1ABC626FBADBC1795A34F706C643F6238B3DDC7902C73E62BD4CEEF5C39D58411AA2AD908DAC99FABA38C304255251B413B33747EAA17C967E552DA5A29A3294F849C7856A71C20AB6E1824B7466180BC5C465C2FD574813202E5A242465A330B357DD051D057CB664249AA9914912A85B3E4AE69AAA7D218A691809A9DA54CF5158E54AA9B1251A6945B5A9CA84CCBDA08681269A30DD7D148C9C74A0ADD66620C64529920BA4F6927799F194EE5003BE8BAED34ABB364B28D36A82DA6F13236B75BC9DD29EA4E92328F3549834CCBE26F7DC9217D8B92207CF6D7258CBD754562496822E830C7F3B2CD157A0A8BEB02C751D1840BDC5F430C5C72763F8FB0F7041C4CAA1D18C7E9AF3EBE017F4B9A7C0C1EA17B856EB678B3C564CA3078F4995D33B96DD48D9FA69366795EDEA4EF2DE23EA640D8F492770B37E8C3D6F3DD92EF4FE24B081589E41A93BF5249D61227AF55D66F25A52F21D224948BAFBC7DDDC180582286F10D5A8117D886B7FB187E866BE0BC153FD95613695E0856ECCB4B0FAC2310C4398DAA3FF993E8B01BBCFEF47FE57F960D6CA90000 , N'6.1.3-40302')