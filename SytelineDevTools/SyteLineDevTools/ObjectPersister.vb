﻿Imports Mongoose.IDO.Protocol
Imports Mongoose.WinStudio.Enums
Imports Mongoose.Core.Common 
Imports System.IO

Public Module ObjectPersister
    Public Sub PersistObject(baserepopath As String, objecttype As GlobalObjectType,objectname As String,xmlobj As XmlSerializableObject)
        CheckDirectory(baserepopath)
        Dim objectdir = Path.Combine(baserepopath,[Enum].GetName(GetType(GlobalObjectType),objecttype))
        CheckDirectory(objectdir)
        Dim xmlfilename = Path.Combine(objectdir,Path.ChangeExtension(objectname,"xml")).Replace("/","-")
        File.WriteAllText(xmlfilename,xmlobj.ToFormattedXml)
    End Sub

    Private Sub CheckDirectory(dir As string)
        If Not Directory.Exists(dir) Then
            Directory.CreateDirectory(dir)
        End If
    End Sub

End Module
